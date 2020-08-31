using System;
using DFlow.Base;
using DFlow.Bus;
using DFlow.Example;
using DFlow.Example.Aggregates;
using DFlow.Example.Commands;
using DFlow.Example.Events;
using DFlow.Example.Handlers;
using DFlow.Example.Views;
using DFlow.Interfaces;
using Xunit;

namespace DFlow.Tests
{
    public class HandlerTests
    {
        private readonly IAppendOnlyStore<Guid> _appendOnly;
        private IEventBus _eventBus;
        private readonly IEventStore<Guid> _eventStore;
        private ISnapshotRepository<Guid> _snapShotRepo;
        private readonly AggregateFactory _factory;
        private readonly MemoryResolver _resolver;
        
        public HandlerTests()
        {
            _resolver = new MemoryResolver();
            _eventBus = new MemoryEventBus(_resolver);
            _appendOnly = new MemoryAppendOnlyStore(_eventBus);
            _eventStore = new EventStore(_appendOnly, _eventBus);
            _snapShotRepo = new SnapshotRepository();
            _factory = new AggregateFactory(_eventStore, _snapShotRepo);
        }
        
        [Fact]
        public void ShouldCreateProductCatalog()
        {
            var rootId = Guid.NewGuid();
            var handler = new ProductServiceCommandHandler(_eventStore, _factory);
            var view = new ProductView();
            _resolver.Register<ProductCreated>(view);
            
            handler.Execute(new CreateProductCatalog(rootId));
            handler.Execute(new CreateProductCommand(rootId, Guid.NewGuid(), "Notebook Lenovo 2 em 1 ideapad C340", "Notebook Lenovo 2 em 1 ideapad C340 i7-8565U 8GB 256GB SSD Win10 14' FHD IPS - 81RL0001BR"));

            var stream = _eventStore.LoadEventStream(rootId);
            var productAggregate = new ProductCatalogAggregate(stream);
            
            Assert.True(stream.Version == 2);
            Assert.True(1 == view.Products.Count);
        }
    }
}
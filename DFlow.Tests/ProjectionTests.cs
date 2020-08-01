using System;
using DFlow.Bus;
using DFlow.Example;
using DFlow.Example.Commands;
using DFlow.Example.Handlers;
using DFlow.Interfaces;
using DFlow.Store;
using Xunit;

namespace DFlow.Tests
{
    public class ProjectionTests
    {
        private IAppendOnlyStore<Guid> _appendOnly = null;
        private IEventBus _eventBus = null;
        private IEventStore<Guid> _eventStore = null;
        private ISnapshotRepository<Guid> _snapShotRepo = null;
        private AggregateFactory _factory = null;
        private ViewFactory _viewFactory = null;
        
        public ProjectionTests()
        {
            _eventBus = new MemoryEventBus();
            _appendOnly = new MemoryAppendOnlyStore(_eventBus);
            _eventStore = new EventStore(_appendOnly);
            _snapShotRepo = new SnapshotRepository();
            _factory = new AggregateFactory(_eventStore, _snapShotRepo);
            _viewFactory = new ViewFactory(_eventBus);
        }
        
        [Fact]
        public void ShouldUpdateProductCatalogView()
        {
            var rootId = Guid.NewGuid();
            var handler = new ProductServiceCommandHandler(_eventStore, _factory);
            
            var idProd2 = Guid.NewGuid();
            
            IProductQueryHandler queryHandler = new ProductQueryHandler(_viewFactory);

            handler.Execute(new CreateProductCatalog(rootId));
            handler.Execute(new CreateProductCommand(rootId, Guid.NewGuid(), "Notebook Lenovo 2 em 1 ideapad C340", "Notebook Lenovo 2 em 1 ideapad C340 i7-8565U 8GB 256GB SSD Win10 14' FHD IPS - 81RL0001BR"));
            handler.Execute(new CreateProductCommand(rootId, idProd2, "Notebook 2 em 1 Dell", "Notebook Lenovo 2 em 1 ideapad C340 i7-8565U 8GB 256GB SSD Win10 14' FHD IPS - 81RL0001BR"));

            var product = queryHandler.GetById(idProd2);
            var listProducts = queryHandler.ListAllProducts();
            var dell = queryHandler.ListByFilter(x => x.Name.Contains("Dell"));

            
            Assert.True(product.Id == idProd2);
            Assert.True(listProducts.Count == 2);
            Assert.True(dell.Count == 1);
        }
        
        [Fact]
        public void Dispose()
        {
            _appendOnly = null;
            _eventBus = null;;
            _eventStore = null;;
            _snapShotRepo = null;;
            _factory = null;;
        }
    }
}
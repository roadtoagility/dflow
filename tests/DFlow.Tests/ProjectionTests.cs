using System;
using DFlow.Base;
using DFlow.Bus;
using DFlow.Example;
using DFlow.Example.Commands;
using DFlow.Example.Events;
using DFlow.Example.Handlers;
using DFlow.Example.Views;
using DFlow.Interfaces;
using Xunit;

namespace DFlow.Tests
{
    public class ProjectionTests
    {
        private readonly IEventStore<Guid> _eventStore;
        private readonly AggregateFactory _factory;
        private readonly MemoryResolver _resolver;
        private readonly IAppendOnlyStore<Guid> _appendOnly;
        private readonly IEventBus _eventBus;
        private readonly ISnapshotRepository<Guid> _snapShotRepo;

        public ProjectionTests()
        {
            _resolver = new MemoryResolver();
            _eventBus = new MemoryEventBus(_resolver);
            _appendOnly = new MemoryAppendOnlyStore(_eventBus);
            _eventStore = new EventStore(_appendOnly, _eventBus);
            _snapShotRepo = new SnapshotRepository();
            _factory = new AggregateFactory(_eventStore, _snapShotRepo);
        }

        [Fact]
        public void ShouldUpdateProductCatalogView()
        {
            var rootId = Guid.NewGuid();
            var handler = new ProductServiceCommandHandler(_eventStore, _factory);

            var idProd2 = Guid.NewGuid();
            var view = new ProductView();
            _resolver.Register<ProductCreated>(view);

            IProductQueryHandler queryHandler = new ProductQueryHandler(view);

            handler.Execute(new CreateProductCatalog(rootId));
            handler.Execute(new CreateProductCommand(rootId, Guid.NewGuid(), "Notebook Lenovo 2 em 1 ideapad C340",
                "Notebook Lenovo 2 em 1 ideapad C340 i7-8565U 8GB 256GB SSD Win10 14' FHD IPS - 81RL0001BR"));
            handler.Execute(new CreateProductCommand(rootId, idProd2, "Notebook 2 em 1 Dell",
                "Notebook Lenovo 2 em 1 ideapad C340 i7-8565U 8GB 256GB SSD Win10 14' FHD IPS - 81RL0001BR"));

            var product = queryHandler.GetById(idProd2);
            var listProducts = queryHandler.ListAllProducts();
            var dell = queryHandler.ListByFilter(x => x.Name.Contains("Dell"));


            Assert.True(product.Id == idProd2);
            Assert.True(listProducts.Count == 2);
            Assert.True(dell.Count == 1);
        }
    }
}
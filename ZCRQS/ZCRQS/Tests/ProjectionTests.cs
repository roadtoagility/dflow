using System;
using Core.Infrastructure.Queue.Memory;
using Core.Infrastructure.Write.Memory;
using Core.Shared.Interfaces;
using Program.Aggregates;
using Program.Commands;
using Program.Handlers;
using Xunit;

namespace Program.Tests
{
    public class ProjectionTests
    {
        private IAppendOnlyStore<Guid> _appendOnly = null;
        private IQueueService _queueService = null;
        private IEventStore<Guid> _eventStore = null;
        private ISnapshotRepository<Guid> _snapShotRepo = null;
        private AggregateFactory _factory = null;
        private ViewFactory _viewFactory = null;
        
        public ProjectionTests()
        {
            _queueService = new MemoryQueueService();
            _appendOnly = new MemoryAppendOnlyStore(_queueService);
            _eventStore = new EventStore(_appendOnly, _queueService);
            _snapShotRepo = new SnapshotRepository();
            _factory = new AggregateFactory(_eventStore, _snapShotRepo);
            _viewFactory = new ViewFactory(_queueService);
        }
        
        [Fact]
        public void ShouldUpdateProductCatalogView()
        {
            var rootId = Guid.NewGuid();
            var handler = new ProductServiceCommandHandler(_eventStore, _factory);

            var idProd2 = Guid.NewGuid();
            handler.Execute(new CreateProductCatalog(rootId));
            handler.Execute(new CreateProductCommand(rootId, Guid.NewGuid(), "Notebook Lenovo 2 em 1 ideapad C340", "Notebook Lenovo 2 em 1 ideapad C340 i7-8565U 8GB 256GB SSD Win10 14' FHD IPS - 81RL0001BR"));
            handler.Execute(new CreateProductCommand(rootId, idProd2, "Notebook 2 em 1 Dell", "Notebook Lenovo 2 em 1 ideapad C340 i7-8565U 8GB 256GB SSD Win10 14' FHD IPS - 81RL0001BR"));


            IProductQueryHandler queryHandler = new ProductQueryHandler(_viewFactory);
            
            var product = queryHandler.GetById(idProd2);
            var listProducts = queryHandler.ListAllProducts();
            var dell = queryHandler.ListByFilter(x => x.Name.Contains("Dell"));
            
            Assert.True(product.Id == idProd2);
            Assert.True(listProducts.Count == 2);
            Assert.True(dell.Count == 1);
        }
        
        public void Dispose()
        {
            _appendOnly = null;
            _queueService = null;;
            _eventStore = null;;
            _snapShotRepo = null;;
            _factory = null;;
        }
    }
}
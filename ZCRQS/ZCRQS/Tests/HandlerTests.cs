using System;
using Core.Shared.Base.Aggregate;
using Core.Shared.Interfaces;
using Program.Aggregates;
using Program.Commands;
using Program.Handlers;
using Xunit;

namespace Program.Tests
{
    public class HandlerTests : IDisposable
    {
        private IAppendOnlyStore<Guid> appendOnly = null;
        private IQueueService queueService = null;
        private IEventStore<Guid> eventStore = null;
        private ISnapshotRepository<Guid> snapShotRepo = null;
        private AggregateFactory factory = null;
        
        public HandlerTests()
        {
            appendOnly = new MemoryAppendOnlyStore();
            queueService = new MemoryQueueService();
            eventStore = new EventStore(appendOnly, queueService);
            snapShotRepo = new SnapshotRepository();
            factory = new AggregateFactory(eventStore, snapShotRepo);
        }
        
        [Fact]
        public void ShouldCreateProductCatalog()
        {
            var rootId = Guid.NewGuid();
            var handler = new ProductServiceCommandHandler(eventStore, factory);
            
            handler.Execute(new CreateProductCatalog(rootId));
            handler.Execute(new CreateProductCommand(rootId, Guid.NewGuid(), "Notebook Lenovo 2 em 1 ideapad C340", "Notebook Lenovo 2 em 1 ideapad C340 i7-8565U 8GB 256GB SSD Win10 14' FHD IPS - 81RL0001BR"));

            var stream = eventStore.LoadEventStream(rootId);
            var productAggregate = new ProductCatalogAggregate(stream);
            
            Assert.True(stream.Version == 2);
            Assert.True(1 == productAggregate.CountProducts());
        }

        public void Dispose()
        {
            appendOnly = null;
            queueService = null;;
            eventStore = null;;
            snapShotRepo = null;;
            factory = null;;
        }
    }
}
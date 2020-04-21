using System;
using Core.Infrastructure.Queue.Memory;
using Core.Infrastructure.Write.Memory;
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
        private IAppendOnlyStore<Guid> _appendOnly = null;
        private IQueueService _queueService = null;
        private IEventStore<Guid> _eventStore = null;
        private ISnapshotRepository<Guid> _snapShotRepo = null;
        private AggregateFactory _factory = null;
        
        public HandlerTests()
        {
            _queueService = new MemoryQueueService();
            _appendOnly = new MemoryAppendOnlyStore(_queueService);
            _eventStore = new EventStore(_appendOnly, _queueService);
            _snapShotRepo = new SnapshotRepository();
            _factory = new AggregateFactory(_eventStore, _snapShotRepo);
        }
        
        [Fact]
        public void ShouldCreateProductCatalog()
        {
            var rootId = Guid.NewGuid();
            var handler = new ProductServiceCommandHandler(_eventStore, _factory);
            
            handler.Execute(new CreateProductCatalog(rootId));
            handler.Execute(new CreateProductCommand(rootId, Guid.NewGuid(), "Notebook Lenovo 2 em 1 ideapad C340", "Notebook Lenovo 2 em 1 ideapad C340 i7-8565U 8GB 256GB SSD Win10 14' FHD IPS - 81RL0001BR"));

            var stream = _eventStore.LoadEventStream(rootId);
            var productAggregate = new ProductCatalogAggregate(stream);
            
            Assert.True(stream.Version == 2);
            Assert.True(1 == productAggregate.CountProducts());
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
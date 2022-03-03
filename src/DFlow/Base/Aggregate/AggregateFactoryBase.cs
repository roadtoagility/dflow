using System;
using System.Diagnostics.Tracing;
using DFlow.Interfaces;

namespace DFlow.Base.Aggregate
{
    public abstract class AggregateFactoryBase
    {
        private readonly ISnapshotRepository<Guid>? _snapshotRepository;
        private readonly IEventStore<Guid> _eventStore;

        protected AggregateFactoryBase(IEventStore<Guid> eventStore)
        {
            _eventStore = eventStore;
        }
        
        protected AggregateFactoryBase(IEventStore<Guid> eventStore, ISnapshotRepository<Guid> snapshotRepository)
         : this(eventStore)
        {
            _snapshotRepository = snapshotRepository;
        }
        
        public TAggregate Load<TAggregate>(Guid id)
            where TAggregate : AggregateRoot<Guid>
        {
            TAggregate root;
            long snapshotVersion = 0;
            
            if (_snapshotRepository != null && _snapshotRepository.TryGetSnapshotById<TAggregate>(id, out root, out snapshotVersion))
            {
                var stream = _eventStore.LoadEventStreamAfterVersion(id, snapshotVersion);
                
                root.ReplayEvents(stream.Events);

                return root;
            }
            else 
            {
                EventStream stream = _eventStore.LoadEventStream(id);

                return (TAggregate)Activator.CreateInstance(typeof(TAggregate), new object[] { stream });
            }
        }

        public abstract TAggregate Create<TAggregate>() 
            where TAggregate : AggregateRoot<Guid>;
        
        public abstract TAggregate Create<TAggregate>(Guid id)
            where TAggregate : AggregateRoot<Guid>;
    }
}
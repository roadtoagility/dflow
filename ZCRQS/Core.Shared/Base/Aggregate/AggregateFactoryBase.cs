using System;
using System.Collections.Generic;
using Core.Shared.Base.Exceptions;
using Core.Shared.Interfaces;

namespace Core.Shared.Base.Aggregate
{
    public abstract class AggregateFactoryBase
    {
        private readonly ISnapshotRepository<Guid> _snapshotRepository;
        private readonly IEventStore<Guid> _eventStore;

        //summay: aplicações não são obrigadas a possuirem uma estrutura de snapshot
        public AggregateFactoryBase(IEventStore<Guid> eventStore, ISnapshotRepository<Guid> snapshotRepository = null)
        {
            _snapshotRepository = snapshotRepository;
            _eventStore = eventStore;
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

        public abstract TAggregate Create<TAggregate>(Guid? id = null)
            where TAggregate : AggregateRoot<Guid>;
    }
}
using System;
using System.Collections.Generic;

namespace Core.Shared
{
    public class AggregateFactory
    {
        private readonly ISnapshotRepository _snapshotRepository;
        private readonly IEventStore _eventStore;

        //summay: aplicações não são obrigadas a possuirem uma estrutura de snapshot
        public AggregateFactory(IEventStore eventStore, ISnapshotRepository snapshotRepository = null)
        {
            _snapshotRepository = snapshotRepository;
            _eventStore = eventStore;
        }
        
        public TAggregate Create<TAggregate>(Guid? id = null)
            where TAggregate : AggregateRoot
        {
            id = id == null || id.Value == Guid.Empty ? Guid.NewGuid() : id;
            
            var createEvent = new AggregateCreated(id.Value);
            var events = new List<IEvent>();
            events.Add(createEvent);

            TAggregate aggregate = (TAggregate) Activator.CreateInstance(typeof(TAggregate), new object[] {events});
            
            aggregate.Changes.Add(createEvent);
            return aggregate;
        }
        
        public TAggregate Load<TAggregate>(Guid id)
            where TAggregate : AggregateRoot
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

                return (TAggregate)Activator.CreateInstance(typeof(TAggregate), new object[] { stream.Events });
            }
        }
    }
}
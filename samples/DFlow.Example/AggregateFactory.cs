using System;
using System.Collections.Generic;
using DFlow.Base;
using DFlow.Base.Aggregate;
using DFlow.Base.Exceptions;
using DFlow.Interfaces;

namespace DFlow.Example
{
    public class AggregateFactory : AggregateFactoryBase
    {
        private readonly IEventStore<Guid> _eventStore;
        private readonly long INITIAL_VERSION = 1;

        public AggregateFactory(IEventStore<Guid> eventStore, ISnapshotRepository<Guid> snapshotRepository = null) 
            : base(eventStore, snapshotRepository)
        {
            _eventStore = eventStore;
        }

        public override TAggregate Create<TAggregate>()
        {
            return Create<TAggregate>(Guid.NewGuid());
        }

        public override TAggregate Create<TAggregate>(Guid id)
        {
            var existStream = _eventStore.Any(id);
            
            if (existStream)
            {
                throw new DuplicatedRootException(id.ToString());
            }
            
            var createEvent = new AggregateCreated<Guid>(id);
            var events = new List<IEvent>() { createEvent};
            
            var stream = new EventStream(){ Version = INITIAL_VERSION, Events = events};
            
            TAggregate aggregate = (TAggregate) Activator.CreateInstance(typeof(TAggregate), new object[] {stream});
            
            aggregate.Changes.Add(createEvent);
            return aggregate;
        }
    }
}
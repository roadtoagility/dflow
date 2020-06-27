using System;
using System.Collections.Generic;
using Core.Shared.Base;
using DFlow.Base.Aggregate;
using DFlow.Base.Exceptions;
using DFlow.Interfaces;

namespace Program
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
        
        // public TAggregate Create<TAggregate>(Guid? id = null)
        //     where TAggregate : AggregateRoot<Guid>
        // {
        //     id = id == null || id.Value == Guid.Empty ? Guid.NewGuid() : id;
        //     
        //     var existStream = _eventStore.Any(id.Value);
        //     if(existStream)
        //         throw new DuplicatedRootException(id.ToString());
        //     
        //     var createEvent = new AggregateCreated<Guid>(id.Value);
        //     var events = new List<IEvent>() { createEvent};
        //     
        //     var stream = new EventStream(){ Version = INITIAL_VERSION, Events = events};
        //
        //     TAggregate aggregate = (TAggregate) Activator.CreateInstance(typeof(TAggregate), new object[] {stream});
        //     
        //     aggregate.Changes.Add(createEvent);
        //     return aggregate;
        // }


        public override TAggregate Create<TAggregate>(Guid? id = null)
        {
            id = id == null || id.Value == Guid.Empty ? Guid.NewGuid() : id;
            
            var existStream = _eventStore.Any(id.Value);
            if(existStream)
                throw new DuplicatedRootException(id.ToString());
            
            var createEvent = new AggregateCreated<Guid>(id.Value);
            var events = new List<IEvent>() { createEvent};
            
            var stream = new EventStream(){ Version = INITIAL_VERSION, Events = events};
            
            TAggregate aggregate = (TAggregate) Activator.CreateInstance(typeof(TAggregate), new object[] {stream});
            
            aggregate.Changes.Add(createEvent);
            return aggregate;
        }
    }
}
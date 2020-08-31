using System;
using System.Collections.Generic;
using DFlow.Interfaces;

namespace DFlow.Base.Aggregate
{
    [Serializable]
    public abstract class AggregateRoot<T>
    {
        public IList<IEvent> Changes { get; protected set; }
        public IList<IDomainEvent> DomainEvents { get; protected set; }
        public T Id { get; protected set; }
        public long Version { get; protected set; }
        
        public AggregateRoot(EventStream stream)
        {
            Changes = new List<IEvent>();
            DomainEvents = new List<IDomainEvent>();
            Version = stream.Version;
            ReplayEvents(stream.Events);
        }

        public void ReplayEvents(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                Mutate(@event);
            }
        }
        
        protected void Apply(IEvent @event)
        {
            Changes.Add(@event);
            Mutate(@event);
            Version++;
        }

        protected void Dispatch(IDomainEvent @event)
        {
            DomainEvents.Add(@event);
        }

        protected abstract void Mutate(IEvent e);

    }
}
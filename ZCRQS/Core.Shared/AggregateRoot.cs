using System;
using System.Collections.Generic;

namespace Core.Shared
{
    [Serializable]
    public abstract class AggregateRoot<T>
    {
        public List<IEvent> Changes { get; protected set; }
        public T Id { get; protected set; }
        
        public AggregateRoot(IEnumerable<IEvent> events)
        {
            Changes = new List<IEvent>();
            ReplayEvents(events);
        }

        public void ReplayEvents(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                Mutate(@event);
            }
        }

        protected abstract void Mutate(IEvent e);
    }
}
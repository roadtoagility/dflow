using System;
using Core.Shared.Base.Aggregate;
using Core.Shared.Base.Exceptions;
using Core.Shared.Interfaces;

namespace Core.Shared.Base
{
    public abstract class Handler<T>
    {
        private readonly IEventStore<Guid> _eventStore;

        protected Handler(IEventStore<Guid> eventStore)
        {
            _eventStore = eventStore;
        }

        public virtual bool HasConflict(IEvent event1, IEvent event2)
        {
            return event1.GetType() == event2.GetType();
        }
        
        public virtual void Execute(ICommand command)
        {
            ((dynamic)this).When((dynamic)command);
        }

        //Dessa forma, os métodos HasConflict e HandleConcurrencyException podem ser sobrescritos para se adaptar a questões de negócio
        public virtual void HandleConcurrencyException<TAggregate>(EventStoreConcurrencyException ex, TAggregate root)
            where TAggregate : AggregateRoot<Guid>
        {
            foreach (var changeEvent in root.Changes)
            {
                foreach (var storeEvent in ex.StoreEvents)
                {
                    if (HasConflict(changeEvent, storeEvent))
                    {
                        var msg = string.Format("Conflict between {0} and {1}",
                            changeEvent, changeEvent);
                        throw new Exception(msg, ex);
                    }
                }
            }
                    
            var actualVersion = root.Version - root.Changes.Count;
            actualVersion += actualVersion;
            _eventStore.AppendToStream<TAggregate>(root.Id, actualVersion, root.Changes);
        }
    }
}
using System;
using System.Collections.Generic;
using DFlow.Base.Aggregate;
using DFlow.Base.Exceptions;
using DFlow.Interfaces;

namespace Core.Shared.Base
{
    public abstract class Handler<T>
    {
        private readonly IEventStore<Guid> _eventStore;

        protected Handler(IEventStore<Guid> eventStore)
        {
            _eventStore = eventStore;
        }

        protected virtual bool HasConflict(IEvent event1, IEvent event2)
        {
            return event1.GetType() == event2.GetType();
        }
        
        public virtual Result<object> Execute(ICommand command)
        {
            return ((dynamic)this).When((dynamic)command);
        }

        //Dessa forma, os métodos HasConflict e HandleConcurrencyException podem ser sobrescritos para se adaptar a questões de negócio
        protected virtual void HandleConcurrencyException<TAggregate>(EventStoreConcurrencyException ex, TAggregate root)
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
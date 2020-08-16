using System;
using System.Collections.Generic;
using System.Linq;
using DFlow.Configuration;
using DFlow.Interfaces;

namespace DFlow.Bus
{
    public class MemoryEventBus : IEventBus
    {
        private readonly IDependencyResolver _resolver;

        public MemoryEventBus(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }
        
        // public void Subscribe<T>(ISubscriber<T> subscriber)
        // {
        //     _resolver.Register(subscriber);
        // }
        //
        // public void Unsubscribe<T>(ISubscriber<T> subscriber)
        // {
        //     _resolver.Unregister(subscriber);
        // }

        public void Publish(params IEvent[] events)
        {
            foreach (var @event in events)
            {
                var subscribers = _resolver.Resolve(@event.GetType());
                
                foreach (var subscriber in subscribers)
                {
                    ((dynamic)subscriber).Update((dynamic)@event);
                }
            }
        }
    }
}
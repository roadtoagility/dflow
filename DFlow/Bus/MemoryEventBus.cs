using System;
using System.Collections.Generic;
using System.Linq;
using DFlow.Configuration;
using DFlow.Interfaces;

namespace DFlow.Bus
{
    public class MemoryEventBus : IEventBus
    {
        private readonly IDictionary<Type, List<object>> _subscribers;
        
        public MemoryEventBus()
        {
            _subscribers = new Dictionary<Type, List<object>>();
        }
        
        public void Subscribe<T>(ISubscriber<T> subscriber)
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type))
            {
                _subscribers.Add(type, new List<object>());
            }
            
            _subscribers[type].Add(subscriber);
        }

        public void Subscribe(params object[] subscribers)
        {
            foreach (var subscriber in subscribers)
            {
                var type = subscriber.GetType();
                
                if (!_subscribers.ContainsKey(type))
                {
                    _subscribers.Add(type, new List<object>());
                }
            
                _subscribers[type].Add(subscriber);
            }
        }

        public void Unsubscribe<T>(ISubscriber<T> subscriber)
        {
            var type = typeof(T);
            if (_subscribers.ContainsKey(type))
            {
                var subscriberToRemove = _subscribers[type].FirstOrDefault(x => ((ISubscriber<T>)x).GetSubscriberId().Equals(subscriber.GetSubscriberId()));
                if (subscriberToRemove != null) _subscribers[type].Remove(subscriberToRemove);
            }
        }

        public void Publish(params IEvent[] events)
        {
            foreach (var @event in events)
            {
                if (_subscribers.ContainsKey(@event.GetType()))
                {
                    foreach (var subscriber in _subscribers[@event.GetType()])
                    {
                        ((dynamic)subscriber).Update((dynamic)@event);
                    }
                }
            }
        }
    }
}
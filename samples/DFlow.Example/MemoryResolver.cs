using System;
using System.Collections.Generic;
using System.Linq;
using DFlow.Configuration;
using DFlow.Interfaces;

namespace DFlow.Example
{
    public class MemoryResolver : IDependencyResolver
    {
        private readonly IDictionary<Type, List<object>> _subscribers;

        public MemoryResolver()
        {
            _subscribers = new Dictionary<Type, List<object>>();
        }

        public IEnumerable<object> Resolve(Type service)
        {
            // var subscriberType = typeof(ISubscriber<>);
            // var subDefinition = subscriberType.MakeGenericType(service);

            if (!_subscribers.ContainsKey(service)) return new List<object>();

            return _subscribers[service];
        }

        public void Register<T>(ISubscriber<T> subscriber)
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type)) _subscribers.Add(type, new List<object>());

            _subscribers[type].Add(subscriber);
        }

        public void Unregister<T>(ISubscriber<T> subscriber)
        {
            var type = typeof(T);
            if (_subscribers.ContainsKey(type))
            {
                var subscriberToRemove = _subscribers[type].FirstOrDefault(x =>
                    ((ISubscriber<T>)x).GetSubscriberId().Equals(subscriber.GetSubscriberId()));
                if (subscriberToRemove != null) _subscribers[type].Remove(subscriberToRemove);
            }
        }
    }
}
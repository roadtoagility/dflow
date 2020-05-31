using System;
using System.Collections.Generic;
using System.Linq;
using Core.Shared.Interfaces;

namespace Core.Infrastructure.Queue.Memory
{
    public class MemoryQueueService : IQueueService
    {
        private readonly IDictionary<Type, IList<ISubscriber>> _subscribers;
        private readonly IDictionary<string, IList<IEvent>> _messages;

        public MemoryQueueService()
        {
            _messages = new Dictionary<string, IList<IEvent>>();
            _subscribers = new Dictionary<Type, IList<ISubscriber>>();
        }
        
        public void Subscribe<T>(ISubscriber subscriber) 
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type))
            {
                _subscribers.Add(type, new List<ISubscriber>());
            }
            
            //TODO: eu sei que a verificação é desnecessária, só fiz pra garantir que ninguem vai se inscrever várias vezes para o mesmo evento... pode ser removido
            if(!_subscribers[type].Any(x => x.GetSubscriberId().Equals(subscriber.GetSubscriberId())))
            {
                _subscribers[type].Add(subscriber);
            }
            
            ReplayEvents<T>(subscriber);
        }

        public void ReplayEvents<T>(ISubscriber subscriber)
        {
            var key = $"{typeof(T).ToString()}{subscriber.GetSubscriberId()}";
            if (_messages.ContainsKey(key))
            {
                var messages = _messages[subscriber.GetSubscriberId()];

                foreach (var message in messages)
                {
                    subscriber.Update(message);
                }
            
                _messages[subscriber.GetSubscriberId()] = new IEvent[0];
            }
        }
        
        public void Unsubscribe<T>(ISubscriber subscriber) 
        {
            var type = typeof(T);
            if (_subscribers.ContainsKey(type))
            {
                var subscriberToRemove = _subscribers[type].FirstOrDefault(x => x.GetSubscriberId().Equals(subscriber.GetSubscriberId()));
                if (subscriberToRemove != null) _subscribers[type].Remove(subscriberToRemove);
            }
            //TODO: testar isso aqui :)
            var key = $"{typeof(T).ToString()}{subscriber.GetSubscriberId()}";
            if (_messages.ContainsKey(key))
            {
                var messages = _messages[key];
                var messagesToRemove = new List<IEvent>();
                
                foreach (var message in messages)
                {
                    if (message.GetType() == typeof(T))
                    {
                        messagesToRemove.Add(message);
                    }
                }

                foreach (var message in messagesToRemove)
                {
                    _messages[key].Remove(message);
                }
            }
        }

        public void Publish(params IEvent[] events)
        {
            //TODO: ficou horrível isso, melhorar
            foreach (var @event in events)
            {
                if (_subscribers.ContainsKey(@event.GetType()))
                {
                    foreach (var subscriber in _subscribers[@event.GetType()])
                    {
                        subscriber.Update(@event);
                    }
                }
            }
        }

        public void RegisterSubscribers()
        {
            var subscribers = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(ISubscriber).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => x.Name).ToList();
        }
    }
}
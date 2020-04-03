using System;
using System.Collections.Generic;
using System.Linq;
using Core.Shared;
using Core.Shared.Interfaces;

namespace Program
{
    public class MemoryQueueService : IQueueService
    {
        private readonly IDictionary<Type, IList<ISubscriber>> _subscribers;

        public MemoryQueueService()
        {
            _subscribers = new Dictionary<Type, IList<ISubscriber>>();
        }
        
        //TODO: mudar, precisa forçar que o generic implementa a interface T, mudar para IEvent<T>
        public void Subscribe<T>(ISubscriber subscriber) 
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type))
            {
                _subscribers.Add(type, new List<ISubscriber>());
            }
            
            //TODO: eu sei que a verificação é desnecessária, só fiz pra garantir que ninguem vai se inscrever várias vezes para o mesmo evento... pode ser removido
            if(!_subscribers[type].Any(x => x.GetSubscriberId() == subscriber.GetSubscriberId()))
                _subscribers[type].Add(subscriber);
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
    }
}
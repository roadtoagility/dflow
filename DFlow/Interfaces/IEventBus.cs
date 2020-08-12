using System;

namespace DFlow.Interfaces
{
    public interface IEventBus
    {
        void Subscribe<T>(ISubscriber<T> subscriber);
        void Subscribe(params object[] subscribers);
        void Unsubscribe<T>(ISubscriber<T> subscriber); 
        void Publish(params IEvent[] events);
    }
}
using System;

namespace DFlow.Interfaces
{
    public interface IEventBus
    {   
        // void Subscribe<T>(ISubscriber<T> subscriber);
        // void Unsubscribe<T>(ISubscriber<T> subscriber); 
        void Publish(params IEvent[] events);
    }
}
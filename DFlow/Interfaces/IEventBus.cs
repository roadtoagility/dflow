namespace DFlow.Interfaces
{
    public interface IEventBus
    {
        void Subscribe<T>(ISubscriber<T> subscriber) where T: IEvent;
        void Unsubscribe<T>(ISubscriber<T> subscriber) where T: IEvent; 
        void Publish(params IEvent[] events);
    }
}
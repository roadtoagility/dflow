namespace DFlow.Interfaces
{
    public interface IEventBus
    {
        void Subscribe<T>(ISubscriber subscriber);
        void Unsubscribe<T>(ISubscriber subscriber); 
        void Publish(params IEvent[] events);
    }
}
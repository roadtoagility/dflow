namespace DFlow.Interfaces
{
    public interface IEventBus
    {
        void Subscribe<T>(ISubscriber subscriber);
        void Unsubscribe<T>(ISubscriber subscriber); 
        void Publish(params IEvent[] events);

        //TODO: totalmente inutil e feio, remover, não tem função.
        void RegisterSubscribers();
    }
}
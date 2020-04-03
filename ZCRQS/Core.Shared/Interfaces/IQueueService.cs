namespace Core.Shared.Interfaces
{
    public interface IQueueService
    {
        void Subscribe<T>(ISubscriber subscriber);
        void Publish(params IEvent[] events);
    }
}
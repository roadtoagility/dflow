namespace DFlow.Interfaces
{
    public interface ISubscriber<T>
        where T: IEvent
    {
        void Update(T @event);
        string GetSubscriberId();
    }
}
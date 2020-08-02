namespace DFlow.Interfaces
{
    public interface ISubscriber<in T>
    {
        void Update(T @event);
        string GetSubscriberId();
    }
}
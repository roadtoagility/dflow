namespace DFlow.Interfaces
{
    public interface ISubscriber
    {
        void Update(IEvent @event);
        string GetSubscriberId();
    }
}
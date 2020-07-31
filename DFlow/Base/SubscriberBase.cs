using DFlow.Interfaces;

namespace DFlow.Base
{
    public abstract class SubscriberBase : ISubscriber
    {
        public virtual void Update(IEvent @event)
        {
            ((dynamic) this).When((dynamic)@event);
        }

        public abstract string GetSubscriberId();
    }
}
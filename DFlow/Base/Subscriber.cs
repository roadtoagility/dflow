using DFlow.Interfaces;

namespace DFlow.Base
{
    public abstract class Subscriber
    {
        public virtual void Update<T>(T @event)  where T: IEvent
        {
            ((dynamic) this).When((dynamic)@event);
        }

        public abstract string GetSubscriberId();
    }
}
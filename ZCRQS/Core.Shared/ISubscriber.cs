using System;

namespace Core.Shared
{
    public interface ISubscriber
    {
        void Update(IEvent @event);
        Guid GetSubscriberId();
    }
}
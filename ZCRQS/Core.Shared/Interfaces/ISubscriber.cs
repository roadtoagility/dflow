using System;

namespace Core.Shared.Interfaces
{
    public interface ISubscriber
    {
        void Update(IEvent @event);
        Guid GetSubscriberId();
    }
}
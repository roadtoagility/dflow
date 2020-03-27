using System;

namespace Core.Shared
{
    public interface IEvent
    {
        string GetEventName();
        string GetEntityType();

        Guid GetEventId();

        string GetEventType();

        string GetEventDate();
        
        Guid GetRoot();

        string GetEventData();
    }
}
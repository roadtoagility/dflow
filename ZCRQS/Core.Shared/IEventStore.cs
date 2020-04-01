using System;
using System.Collections.Generic;

namespace Core.Shared
{
    public interface IEventStore
    {
        EventStream LoadEventStream(Guid id);

        EventStream LoadEventStream(Guid id, int skipEvents, int maxCount);
        
        void AppendToStream<T>(Guid id, int expectedVersion, ICollection<IEvent> events);
    }
}
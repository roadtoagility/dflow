using System;
using System.Collections.Generic;

namespace Core.Shared
{
    public interface IEventStore
    {
        EventStream LoadEventStream(Guid id);

        EventStream LoadEventStreamAfterVersion(Guid id, long snapshotVersion);
        
        void AppendToStream<T>(Guid id, int version, ICollection<IEvent> events);
    }
}
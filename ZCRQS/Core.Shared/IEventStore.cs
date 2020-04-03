using System;
using System.Collections.Generic;

namespace Core.Shared
{
    public interface IEventStore<TKey>
    {
        EventStream LoadEventStream(TKey id);

        EventStream LoadEventStreamAfterVersion(TKey id, long snapshotVersion);
        
        void AppendToStream<TType>(TKey id, long version, ICollection<IEvent> events);
    }
}
using System;
using System.Collections.Generic;
using Core.Shared.Base;

namespace Core.Shared.Interfaces
{
    public interface IEventStore<TKey>
    {
        EventStream LoadEventStream(TKey id);

        EventStream LoadEventStreamAfterVersion(TKey id, long snapshotVersion);
        
        void AppendToStream<TType>(TKey id, long version, ICollection<IEvent> events);
    }
}
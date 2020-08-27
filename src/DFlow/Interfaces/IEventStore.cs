using System.Collections.Generic;
using DFlow.Base;

namespace DFlow.Interfaces
{
    public interface IEventStore<TKey>
    {
        EventStream LoadEventStream(TKey id);

        EventStream LoadEventStreamAfterVersion(TKey id, long snapshotVersion);
        
        void AppendToStream<TType>(TKey id, long version, ICollection<IEvent> events);

        bool Any(TKey id);
    }
}
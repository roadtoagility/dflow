using System;
using System.Collections.Generic;

namespace Core.Shared
{
    public interface IEventStore
    {

        // loads all events for a stream

        EventStream LoadEventStream(Guid id);

        // loads subset of events for a stream

        EventStream LoadEventStream(Guid id, int skipEvents, int maxCount);

        // appends events to a stream, throwing

        // OptimisticConcurrencyException another appended

        // new events since expectedversion

        void AppendToStream(Guid id, int expectedVersion, ICollection<IEvent> events);

    }
}
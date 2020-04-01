using System.Collections.Generic;

namespace Core.Shared
{
    public class EventStream
    {
        // version of the event stream returned

        public int Version;

        // all events in the stream

        public List<IEvent> Events = new List<IEvent>();
    }
}
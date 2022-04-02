using System.Collections.Generic;
using DFlow.Interfaces;

namespace DFlow.Base
{
    public class EventStream
    {
        public List<IEvent> Events = new List<IEvent>();
        public long Version;
    }
}
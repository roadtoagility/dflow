using System.Collections.Generic;
using DFlow.Interfaces;

namespace Core.Shared.Base
{
    public class EventStream
    {
        public long Version;
        public List<IEvent> Events = new List<IEvent>();
    }
}
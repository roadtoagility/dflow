using System.Collections.Generic;
using Core.Shared.Interfaces;

namespace Core.Shared.Base
{
    public class EventStream
    {
        public long Version;
        public List<IEvent> Events = new List<IEvent>();
    }
}
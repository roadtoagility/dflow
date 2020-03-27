using System;
using System.Collections.Generic;

namespace Core.Shared
{
    public class EventStoreConcurrencyException : Exception
    {
        public List<IEvent> StoreEvents { get; set; }

        public long StoreVersion { get; set; }
    }
}
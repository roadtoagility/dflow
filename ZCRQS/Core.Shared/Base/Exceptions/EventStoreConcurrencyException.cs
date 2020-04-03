using System;
using System.Collections.Generic;
using Core.Shared.Interfaces;

namespace Core.Shared.Base.Exceptions
{
    public class EventStoreConcurrencyException : Exception
    {
        public List<IEvent> StoreEvents { get; set; }

        public long StoreVersion { get; set; }
    }
}
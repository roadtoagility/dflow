using System;
using Core.Shared;

namespace Program.Events
{
    public class ProductCatalogAggregateCreate : IEvent
    {
        public Guid Id { get; private set; }

        public ProductCatalogAggregateCreate(Guid id)
        {
            Id = id;
        }

        public string GetEventName()
        {
            throw new NotImplementedException();
        }

        public string GetEntityType()
        {
            throw new NotImplementedException();
        }

        public Guid GetEventId()
        {
            throw new NotImplementedException();
        }

        public string GetEventType()
        {
            throw new NotImplementedException();
        }

        public string GetEventDate()
        {
            throw new NotImplementedException();
        }

        public Guid GetRoot()
        {
            throw new NotImplementedException();
        }

        public string GetEventData()
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using Core.Shared;

namespace Program.Events
{
    [Serializable]
    public class ProductCatalogAggregateCreated : IEvent
    {
        public Guid Id { get; private set; }
        
        public ProductCatalogAggregateCreated(Guid id)
        {
            Id = id;
        }
    }
}
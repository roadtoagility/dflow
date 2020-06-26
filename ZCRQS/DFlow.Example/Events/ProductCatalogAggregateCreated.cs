using System;
using Core.Shared;
using DFlow.Interfaces;

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
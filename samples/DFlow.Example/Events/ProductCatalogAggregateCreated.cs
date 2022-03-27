using System;
using DFlow.Interfaces;

namespace DFlow.Example.Events
{
    [Serializable]
    public class ProductCatalogAggregateCreated : IDomainEvent
    {
        public ProductCatalogAggregateCreated(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }
}
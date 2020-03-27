using System;
using System.Collections.Generic;
using Core.Shared;
using Program.Commands;
using Program.Events;

namespace Program.Aggregates
{
    public class ProductCatalogAggregate
    {
        public List<IEvent> Changes { get; private set; }
        public Guid Id { get; private set; }

        public ProductCatalogAggregate(IEnumerable<IEvent> events)
        {
            Changes = new List<IEvent>();
            
            foreach (var @event in events)
            {
                Mutate(@event);
            }
        }
        
        public void CreateProduct(CreateProductCommand cmd)
        {
            Apply(new ProductCreated(Id, cmd));
        }
        
        void Apply(IEvent @event)
        {
            Changes.Add(@event);
            Mutate(@event);
        }
        
        private void Mutate(IEvent e)
        {
            ((dynamic) this).When((dynamic)e);
        }
        
        private void When(ProductCreated e)
        {


        }
    }
}
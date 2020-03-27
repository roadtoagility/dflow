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

        public static ProductCatalogAggregate CreateRoot(Guid id)
        {
            if(id == Guid.Empty)
                throw new Exception("Invalid ID");
            
            var createEvent = new ProductCatalogAggregateCreate(id);
            var events = new List<IEvent>();
            events.Add(createEvent);
            

            var aggregateCreated = new ProductCatalogAggregateCreated();
            events.Add(aggregateCreated);
            
            var root = new ProductCatalogAggregate(events);
            root.Changes.Add(createEvent);
            root.Changes.Add(aggregateCreated);
            return root;
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
        
        private void When(ProductCatalogAggregateCreated e)
        {
            
        }
        
        private void When(ProductCatalogAggregateCreate e)
        {
            if (e.Id != Guid.Empty)
            {
                Id = e.Id;
            }
        }
    }
}
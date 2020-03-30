using System;
using System.Collections.Generic;
using System.Linq;
using Core.Shared;
using Program.Commands;
using Program.Entities;
using Program.Events;

namespace Program.Aggregates
{
    public class ProductCatalogAggregate
    {
        public List<IEvent> Changes { get; private set; }
        public Guid Id { get; private set; }

        private List<Product> _products;

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
            
            var createEvent = new ProductCatalogAggregateCreated(id);
            var events = new List<IEvent>();
            events.Add(createEvent);
            
            var root = new ProductCatalogAggregate(events);
            root.Changes.Add(createEvent);
            return root;
        }
        
        public void CreateProduct(CreateProductCommand cmd)
        {
            if(!_products.Any(x => x.Id == cmd.Id || x.Name.Equals(cmd.Name)))
                Apply(new ProductCreated(cmd.Id, cmd.Name, cmd.Description));
        }

        //TODO: eu sei que isso não deve estar aqui, meramente para testes enquanto não crio as projeções
        public int CountProducts()
        {
            return _products.Count;
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
            _products.Add(new Product(e.Id, e.Name, e.Description));
        }
        
        private void When(ProductCatalogAggregateCreated e)
        {
            if (e.Id != Guid.Empty)
            {
                Id = e.Id;
                _products = new List<Product>();
            }
        }
        
    }
}
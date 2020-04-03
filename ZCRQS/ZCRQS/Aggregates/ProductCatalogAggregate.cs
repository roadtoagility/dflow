using System;
using System.Collections.Generic;
using System.Linq;
using Core.Shared;
using Core.Shared.Base;
using Core.Shared.Base.Aggregate;
using Core.Shared.Interfaces;
using Program.Commands;
using Program.Entities;
using Program.Events;

namespace Program.Aggregates
{
    [Serializable]
    public class ProductCatalogAggregate : AggregateRoot<Guid>
    {
        private List<Product> _products;

        public ProductCatalogAggregate(IEnumerable<IEvent> events)
            : base(events)
        {
            
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
        
        protected override void Mutate(IEvent e)
        {
            ((dynamic) this).When((dynamic)e);
        }
        
        private void When(ProductCreated e)
        {
            _products.Add(new Product(e.Id, e.Name, e.Description));
        }
        
        private void When(AggregateCreated<Guid> e)
        {
            Id = e.Id;
            _products = new List<Product>();
        }
    }
}
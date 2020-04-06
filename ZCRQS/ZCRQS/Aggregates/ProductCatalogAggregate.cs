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

        public ProductCatalogAggregate(EventStream stream)
            : base(stream)
        {
            
        }
        
        public void CreateProduct(CreateProductCommand cmd)
        {
            if(!_products.Any(x => x.Id == cmd.Id || x.Name.Equals(cmd.Name)))
                Apply(new ProductCreated(cmd.Id, cmd.Name, cmd.Description));
        }
        
        public void ChangeProductName(ChangeProductNameCommand cmd)
        {
            if(_products.Any(x => x.Id == cmd.ProductId))
                Apply(new ProductNameChanged(cmd.ProductId, cmd.Name));
        }

        //TODO: eu sei que isso não deve estar aqui, meramente para testes enquanto não crio as projeções
        public int CountProducts()
        {
            return _products.Count;
        }
        
        protected override void Mutate(IEvent e)
        {
            ((dynamic) this).When((dynamic)e);
        }
        
        private void When(ProductCreated e)
        {
            _products.Add(new Product(e.Id, e.Name, e.Description));
            
        }
        
        private void When(ProductNameChanged e)
        {
            var product = _products.Where(x => x.Id == e.Id).FirstOrDefault();
            product.ChangeName(e.Name);
        }
        
        private void When(AggregateCreated<Guid> e)
        {
            Id = e.Id;
            _products = new List<Product>();
        }
    }
}
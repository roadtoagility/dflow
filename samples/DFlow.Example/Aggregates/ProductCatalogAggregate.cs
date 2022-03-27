using System;
using System.Collections.Generic;
using System.Linq;
using DFlow.Base;
using DFlow.Base.Aggregate;
using DFlow.Example.Commands;
using DFlow.Example.Entities;
using DFlow.Example.Events;
using DFlow.Interfaces;

namespace DFlow.Example.Aggregates
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
            if (!_products.Any(x => x.Id == cmd.Id || x.Name.Equals(cmd.Name)))
            {
                var @event = new ProductCreated(cmd.Id, cmd.Name, cmd.Description);
                Apply(@event);
                Dispatch(@event);
            }
        }

        public void ChangeProductName(ChangeProductNameCommand cmd)
        {
            if (_products.Any(x => x.Id == cmd.ProductId))
            {
                var @event = new ProductNameChanged(cmd.ProductId, cmd.Name);
                Apply(@event);
                Dispatch(@event);
            }
        }

        protected override void Mutate(IEvent e)
        {
            ((dynamic)this).When((dynamic)e);
        }

        private void When(ProductCreated e)
        {
            _products.Add(new Product(e.Id, e.Name, e.Description));
        }

        private void When(ProductNameChanged e)
        {
            var product = _products.FirstOrDefault(x => x.Id == e.Id);
            product.ChangeName(e.Name);
        }

        private void When(AggregateCreated<Guid> e)
        {
            Id = e.Id;
            _products = new List<Product>();
        }
    }
}
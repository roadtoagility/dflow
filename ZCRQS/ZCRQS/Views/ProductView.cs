using System;
using System.Collections.Generic;
using System.Linq;
using Core.Shared;
using Core.Shared.Interfaces;
using Program.Entities;
using Program.Events;

namespace Program.Views
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
    }
    
    public class ProductView : ISubscriber
    {
        public List<ProductDTO> Products { get; set; }
        public Guid Id { get; private set; }

        public ProductView()
        {
            Products = new List<ProductDTO>();
            Id = Guid.NewGuid();
        }
        
        public ProductView(List<ProductDTO> products)
        {
            Products = products;
            Id = Guid.NewGuid();
        }

        public void Update(IEvent @event)
        {
            ((dynamic) this).When((dynamic)@event);
        }

        public Guid GetSubscriberId()
        {
            return Id;
        }
        
        private void When(ProductCreated e)
        {
            Products.Add(new ProductDTO() {Name = e.Name, Id = e.Id});
        }
        
        private void When(ProductNameChanged e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            var product = Products.FirstOrDefault(x => x.Id == e.Id);
            if (product != null) product.Name = e.Name;
        }
    }
}
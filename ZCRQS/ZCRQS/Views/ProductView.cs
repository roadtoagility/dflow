using System;
using System.Collections.Generic;
using Core.Shared;
using Core.Shared.Interfaces;
using Program.Entities;
using Program.Events;

namespace Program.Views
{
    public class ProductDTO
    {
        public string Name { get; set; }
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
            Products.Add(new ProductDTO() {Name = e.Name});
        }
    }
}
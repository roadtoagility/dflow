using System;
using System.Collections.Generic;
using System.Linq;
using DFlow.Base;
using DFlow.Interfaces;
using DFlow.Example.Entities;
using DFlow.Example.Events;

namespace DFlow.Example.Views
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
    }
    
    public class ProductView : ISubscriber<ProductCreated>, ISubscriber<ProductNameChanged>
    {
        public List<ProductDTO> Products { get; set; }

        public ProductView()
        {
            Products = new List<ProductDTO>();
        }
        
        // public ProductView(ILiteDbViewModel model, IEntityFrameworkRepository)
        // {
        //     Products = new List<ProductDTO>();
        // }
        
        public ProductView(List<ProductDTO> products)
        {
            Products = products;
        }

        public void Update(ProductCreated e)
        {
            Products.Add(new ProductDTO() {Name = e.Name, Id = e.Id});
        }
        
        public void Update(ProductNameChanged e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            var product = Products.FirstOrDefault(x => x.Id == e.Id);
            if (product != null) product.Name = e.Name;
        }

        public string GetSubscriberId()
        {
            return this.GetType().ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using DFlow.Example.Events;
using DFlow.Interfaces;

namespace DFlow.Example.Views
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
    }

    public class ProductView : ISubscriber<ProductCreated>, ISubscriber<ProductNameChanged>
    {
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

        public List<ProductDTO> Products { get; set; }

        public void Update(ProductCreated e)
        {
            Products.Add(new ProductDTO { Name = e.Name, Id = e.Id });
        }

        public string GetSubscriberId()
        {
            return GetType().ToString();
        }

        public void Update(ProductNameChanged e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            var product = Products.FirstOrDefault(x => x.Id == e.Id);
            if (product != null) product.Name = e.Name;
        }
    }
}
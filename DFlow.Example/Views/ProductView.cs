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
        
        public ProductView(List<ProductDTO> products)
        {
            Products = products;
        }

        public void Update(ProductCreated @event)
        {
            throw new NotImplementedException();
        }

        public void Update(ProductNameChanged @event)
        {
            throw new NotImplementedException();
        }

        public string GetSubscriberId()
        {
            return this.GetType().ToString();
        }
        
        public ProductDTO Query(Func<ProductDTO, bool> query)
        {
            return Products.Where(query).FirstOrDefault();
        }
        
        public IList<ProductDTO> QueryAll(Func<ProductDTO, bool> query = null)
        {
            if (query != null)
                return Products.Where(query).ToList();
            else
                return Products;
        }
        
        public void When(ProductCreated e)
        {
            Products.Add(new ProductDTO() {Name = e.Name, Id = e.Id});
        }
        
        public void When(ProductNameChanged e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            var product = Products.FirstOrDefault(x => x.Id == e.Id);
            if (product != null) product.Name = e.Name;
        }

        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using DFlow.Example.Views;
using DFlow.Interfaces;

namespace DFlow.Example.Handlers
{
    public class ProductQueryHandler : IProductQueryHandler
    {
        private IList<ProductDTO> _products = new List<ProductDTO>();
        public ProductQueryHandler()
        {
        }

        public IList<ProductDTO> ListAllProducts()
        {
            return _products;
        }

        public IList<ProductDTO> ListByFilter(Func<ProductDTO, bool> query)
        {
            return _products.Where(query).ToList();
        }

        public ProductDTO GetById(Guid id)
        {
            return _products.FirstOrDefault(x => x.Id == id);
        }
    }
}
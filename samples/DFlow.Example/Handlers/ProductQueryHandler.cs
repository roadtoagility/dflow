using System;
using System.Collections.Generic;
using System.Linq;
using DFlow.Example.Views;
using DFlow.Interfaces;

namespace DFlow.Example.Handlers
{
    public class ProductQueryHandler : IProductQueryHandler
    {
        private readonly ProductView _view;

        public ProductQueryHandler(ProductView view)
        {
            _view = view;
        }

        public IList<ProductDTO> ListAllProducts()
        {
            return _view.Products;
        }

        public IList<ProductDTO> ListByFilter(Func<ProductDTO, bool> query)
        {
            return _view.Products.Where(query).ToList();
        }

        public ProductDTO GetById(Guid id)
        {
            return _view.Products.FirstOrDefault(x => x.Id == id);
        }
    }
}
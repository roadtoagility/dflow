using System;
using System.Collections.Generic;
using DFlow.Example.Views;

namespace DFlow.Example.Handlers
{
    public class ProductQueryHandler : IProductQueryHandler
    {
        private readonly ViewFactory _factory;

        public ProductQueryHandler(ViewFactory factory)
        {
            _factory = factory;
        }

        public IList<ProductDTO> ListAllProducts()
        {
            var view = _factory.CreateProductView();
            return view.QueryAll();
        }

        public IList<ProductDTO> ListByFilter(Func<ProductDTO, bool> query)
        {
            var view = _factory.CreateProductView();
            return view.QueryAll(query);
        }

        public ProductDTO GetById(Guid id)
        {
            var view = _factory.CreateProductView();
            return view.Query(x => x.Id == id);
        }
    }
}
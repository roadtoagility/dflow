using System;
using System.Collections.Generic;
using DFlow.Example.Views;
using DFlow.Interfaces;

namespace DFlow.Example.Handlers
{
    public class ProductQueryHandler : IProductQueryHandler
    {
        private readonly ViewFactory _factory;
        private IReadModel<ProductDTO> _view;

        public ProductQueryHandler(ViewFactory factory)
        {
            _factory = factory;
            _view = _factory.CreateProductView();
        }

        public IList<ProductDTO> ListAllProducts()
        {
            return _view.QueryAll();
        }

        public IList<ProductDTO> ListByFilter(Func<ProductDTO, bool> query)
        {
            return _view.QueryAll(query);
        }

        public ProductDTO GetById(Guid id)
        {
            return _view.Query(x => x.Id == id);
        }
    }
}
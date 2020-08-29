using System;
using System.Collections.Generic;
using DFlow.Example.Views;

namespace DFlow.Example.Handlers
{
    public interface IProductQueryHandler
    {
        IList<ProductDTO> ListAllProducts();
        
        IList<ProductDTO> ListByFilter(Func<ProductDTO, bool> query);

        ProductDTO GetById(Guid id);
    }
}
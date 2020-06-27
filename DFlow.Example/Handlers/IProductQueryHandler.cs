using System;
using System.Collections.Generic;
using Program.Views;

namespace Program.Handlers
{
    public interface IProductQueryHandler
    {
        IList<ProductDTO> ListAllProducts();
        
        IList<ProductDTO> ListByFilter(Func<ProductDTO, bool> query);

        ProductDTO GetById(Guid id);
    }
}
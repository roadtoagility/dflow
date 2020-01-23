using System.Collections.Generic;
using System.Linq;
using Infraestructure.EntityFramework;
using SharedKernel;

namespace ProductModule.Queries
{
    public class GetAllProducts : IQuery<IEnumerable<Product>>
    {
        
        public GetAllProducts()
        {
            
        }
    }
}
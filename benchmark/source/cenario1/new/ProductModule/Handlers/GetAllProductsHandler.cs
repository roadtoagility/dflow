using System.Collections.Generic;
using System.Linq;
using Infraestructure.EntityFramework;
using ProductModule.Queries;
using SharedKernel;

namespace ProductModule.Handlers
{
    
    public class GetAllProductsHandler : EntityFrameworkConnector, IQueryHandler<GetAllProducts, IEnumerable<Product>>  
    {
        private readonly BenchmarkDBContext _context;

        
        public GetAllProductsHandler(BenchmarkDBContext context)
            :base(context)
        {
            _context = context;
        }
        
        public IEnumerable<Product> Handle(GetAllProducts query)
        {
            return Context.Products.ToList();
        }
    }
}
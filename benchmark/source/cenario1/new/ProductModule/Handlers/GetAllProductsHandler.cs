using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Infraestructure.EntityFramework;
using Microsoft.Data.SqlClient;
using ProductModule.Queries;
using SharedKernel;

namespace ProductModule.Handlers
{
    
    public class GetAllProductsHandler : QueryHandlerBase<GetAllProducts, IEnumerable<Product>>, IQueryHandler//IQueryHandler<GetAllProducts, IEnumerable<Product>>> //, IQueryHandler<GetAllProducts, IEnumerable<Product>>  //EntityFrameworkConnector
    {
        private readonly BenchmarkDBContext _context;

        //private readonly BenchmarkDBContext _context;
        public GetAllProductsHandler(BenchmarkDBContext context)
        {
            _context = context;
        }
        
        // public IEnumerable<Product> Handle(GetAllProducts query)
        // {
        //     return _context.Products.ToList();
        // }

        public override string GetName()
        {
            return "GetAllProductsHandler";
        }

        public override IEnumerable<Product> Handler(GetAllProducts query)
        {

            IEnumerable<Product> products;
            
            using (IDbConnection db = new SqlConnection("server=localhost;database=BD_BENCHMARK;trusted_connection=false;User ID=sa;Password=abc123@abc;MultipleActiveResultSets=true"))
            {

                products = db.Query<Product>("Select * From TB_PRODUCT").ToList();
            }

            return products;
        }
    }
}
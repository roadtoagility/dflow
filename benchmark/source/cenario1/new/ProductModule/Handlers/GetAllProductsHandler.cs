using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Infraestructure.EntityFramework;
using Microsoft.Data.SqlClient;
using ProductModule.Queries;
using SharedKernel;
using SharedKernel.Distribuited;

namespace ProductModule.Handlers
{

    public interface IHandler
    {
        
    }

    public interface IGetAllProductsHandler : IHandler
    {
        IEnumerable<Product> Handle(GetAllProducts query);
    }
    
    public class GetAllProductsHandler : QueryExecutor<GetAllProducts, IEnumerable<Product>>, IGetAllProductsHandler
    {
        private readonly BenchmarkDBContext _context;
        
        public GetAllProductsHandler(BenchmarkDBContext context)
            :base("inproc://GetAllProductsHandler", "")
        {
            _context = context;
        }
        
        // public IEnumerable<Product> Handle(GetAllProducts query)
        // {
        //     return _context.Products.ToList();
        // }

        public override IEnumerable<Product> Handle(GetAllProducts query)
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
using System;
using System.Collections.Generic;
using System.Linq;
using Benchmark.Domain.Spec;
using Benchmark.Repositories.Spec;


namespace Benchmark.Repositories.Impl
{
    public class ProductRepository : IProductRepository
    {
        private readonly BenchmarkDBContext _context;
        
        
        public ProductRepository(BenchmarkDBContext context)
        {
            _context = context;
        }
        
        public IEnumerable<Product> GetProducts()
        {
            return _context.Products.ToList();
        }
    }
}
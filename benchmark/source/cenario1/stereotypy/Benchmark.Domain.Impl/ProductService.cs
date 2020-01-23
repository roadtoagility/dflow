using System;
using System.Collections.Generic;
using Benchmark.Domain.Spec;
using Benchmark.Repositories.Spec;

namespace Benchmark.ProductModule
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        
        public IEnumerable<Product> GetProducts()
        {
            return _productRepository.GetProducts();
        }
    }
}
using System.Collections.Generic;
using Benchmark.Domain.Spec;

namespace Benchmark.Repositories.Spec
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();
    }
}
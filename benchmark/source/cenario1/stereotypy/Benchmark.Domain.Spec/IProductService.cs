using System;
using System.Collections;
using System.Collections.Generic;

namespace Benchmark.Domain.Spec
{
    public interface IProductService
    {
        IEnumerable<Product> GetProducts();
    }
}
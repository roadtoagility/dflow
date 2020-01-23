using System;

namespace Benchmark.Domain.Spec
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        
        protected Product(){}

        public Product(string name, string desc, decimal price)
        {
            Name = name;
            Description = desc;
            Price = price;
        }
    }
}
using System;
using Core.Shared;

namespace Program.Entities
{
    public class Product : Entity<Guid>
    {
        public Product(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
    }
}
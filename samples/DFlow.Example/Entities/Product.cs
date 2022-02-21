using System;
using DFlow.Base;

namespace DFlow.Example.Entities
{
    [Serializable]
    public class Product : Entity<Guid>
    {
        public Product(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public void ChangeName(string name)
        {
            Name = name;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
    }
}
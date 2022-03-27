using System;
using DFlow.Interfaces;

namespace DFlow.Example.Events
{
    [Serializable]
    public class ProductCreated : IDomainEvent
    {
        public ProductCreated(Guid id, string name, string description)
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
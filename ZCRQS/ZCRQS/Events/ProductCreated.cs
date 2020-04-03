using System;
using Core.Shared;
using Core.Shared.Interfaces;
using Program.Commands;

namespace Program.Events
{
    [Serializable]
    public class ProductCreated : IEvent
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }


        public ProductCreated(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
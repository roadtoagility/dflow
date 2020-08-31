using System;
using DFlow.Interfaces;

namespace DFlow.Example.Events
{
    [Serializable]
    public class ProductNameChanged : IDomainEvent
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public ProductNameChanged(Guid id, string name)
        {
            if (id == Guid.Empty || string.IsNullOrEmpty(name))
                throw new Exception("Dados inválidos");
            
            Id = id;
            Name = name;
        }
    }
}
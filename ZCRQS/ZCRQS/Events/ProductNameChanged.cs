using System;
using Core.Shared.Interfaces;

namespace Program.Events
{
    [Serializable]
    public class ProductNameChanged : IEvent
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
using System;
using DFlow.Example.Exceptions;
using DFlow.Interfaces;

namespace DFlow.Example.Commands
{
    public class CreateProductCommand : ICommand
    {
        public Guid RootId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        protected CreateProductCommand(Guid rootId)
        {
            RootId = rootId;
        }

        public CreateProductCommand(Guid rootId, Guid id, string name, string description)
        {
            if (id == Guid.Empty)
            {
                throw new BusinesException("não é possível criar produtos com ID vazio");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new BusinesException("não é possível criar produtos sem nome");
            }

            if (string.IsNullOrEmpty(description))
            {
                throw new BusinesException("não é possível criar produtos sem descrição");
            }

            if (rootId == Guid.Empty)
            {
                throw new BusinesException("não é possível criar produtos sem adiciona-lo a um cátalogo existente");
            }
            
            Description = description;
            RootId = rootId;
            Id = id;
            Name = name;
        }
    }
}
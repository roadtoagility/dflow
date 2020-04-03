using System;
using Core.Shared;
using Core.Shared.Interfaces;

namespace Program.Commands
{
    public class CreateProductCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        protected CreateProductCommand()
        {
            
        }

        public CreateProductCommand(Guid id, string name, string description)
        {
            if(id == Guid.Empty)
                throw new Exception("não é possível criar produtos com ID vazio");
            
            if(string.IsNullOrEmpty(name))
                throw new Exception("não é possível criar produtos sem nome");
            
            if(string.IsNullOrEmpty(description))
                throw new Exception("não é possível criar produtos sem descrição");
            
            Description = description;
            Id = id;
            Name = name;
        }
    }
}
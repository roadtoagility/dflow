using System;
using Core.Shared;

namespace Program.Commands
{
    public class CreateProductCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public CreateProductCommand()
        {
        }

        public CreateProductCommand(Guid id, string name, string description)
        {
            Description = description;
            Id = id;
            Name = name;
        }
    }
}
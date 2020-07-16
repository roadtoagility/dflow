using System;
using DFlow.Interfaces;

namespace DFlow.Example.Commands
{
    
    public class ChangeProductNameCommand : ICommand
    {
        public Guid RootId { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }

        public ChangeProductNameCommand(Guid rootId)
        {
            RootId = rootId;
        }

        public ChangeProductNameCommand(Guid rootId, Guid productId, string name)
        {
            ProductId = productId;
            Name = name;
            RootId = rootId;
        }
    }
}
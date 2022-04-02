using System;
using DFlow.Interfaces;

namespace DFlow.Example.Commands
{
    public class CreateProductCatalog : ICommand
    {
        public CreateProductCatalog(Guid rootId)
        {
            if (rootId == Guid.Empty)
                throw new Exception("não é possível criar catalogos com ID vazio");

            Id = rootId;
        }

        public Guid Id { get; set; }
    }
}
using System;
using DFlow.Interfaces;

namespace Program.Commands
{
    public class CreateProductCatalog : ICommand
    {
        public Guid Id { get; set; }

        public CreateProductCatalog(Guid rootId)
        {
            if(rootId == Guid.Empty)
                throw new Exception("não é possível criar catalogos com ID vazio");

            Id = rootId;
        }
    }
}
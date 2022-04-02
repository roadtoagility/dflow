using System;
using DFlow.Interfaces;

namespace DFlow.Example.Commands
{
    public class AddProductCommand : ICommand
    {
        public AddProductCommand()
        {
        }

        public AddProductCommand(Guid orderId, Guid productId, decimal qtd)
        {
            OrderId = orderId;
            ProductId = productId;
            Qtd = qtd;
        }

        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public decimal Qtd { get; set; }
    }
}
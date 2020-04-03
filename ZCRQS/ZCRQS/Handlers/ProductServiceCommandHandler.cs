using System;
using Core.Shared;
using Core.Shared.Base;
using Core.Shared.Base.Exceptions;
using Core.Shared.Interfaces;
using Program.Aggregates;
using Program.Commands;

namespace Program.Handlers
{
    public class ProductServiceCommandHandler : IProductServiceCommandHandler
    {
        private readonly IEventStore<Guid> _eventStore;

        public ProductServiceCommandHandler(IEventStore<Guid> eventStore)
        {
            _eventStore = eventStore;
        }

        public void Execute(ICommand command)
        {
            ((dynamic)this).When((dynamic)command);
        }

        public void When(CreateProductCommand cmd)
        {
            while(true)
            {
                var stream = _eventStore.LoadEventStream(cmd.Id);
                var productCatalog = new ProductCatalogAggregate(stream.Events);
                
                try
                {
                    
                    // order.AddProduct(cmd.Qtd, cmd.ProductId, _productService);
                    // _eventStore.AppendToStream(cmd.OrderId, stream.Version, order.Changes);
                    return;
                }
                catch (EventStoreConcurrencyException)
                {
                    
                }
            }
        }
    }
}
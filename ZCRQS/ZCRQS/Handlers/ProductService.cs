using Core.Shared;
using Program.Aggregates;
using Program.Commands;

namespace Program.Handlers
{
    public class ProductService : IProducterviceCommandHandler
    {
        private readonly IEventStore _eventStore;

        public ProductService(IEventStore eventStore)
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
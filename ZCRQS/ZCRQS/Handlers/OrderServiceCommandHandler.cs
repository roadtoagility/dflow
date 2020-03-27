using Core.Shared;
using Program.Aggregates;
using Program.Commands;

namespace Program.Handlers
{
    public class OrderServiceCommandHandler : IOrderServiceCommandHandler
    {
        private readonly IEventStore _eventStore;
        private readonly ProductService _productService;
        private readonly CustomerService _customerService;

        public OrderServiceCommandHandler(IEventStore eventStore, ProductService productService, CustomerService customerService)
        {
            _eventStore = eventStore;
            _productService = productService;
            _customerService = customerService;
        }
        
        public void Execute(ICommand command)
        {
            ((dynamic)this).When((dynamic)command);
        }

        public void When(AddProductCommand cmd)
        {
            while(true)
            {
                var stream = _eventStore.LoadEventStream(cmd.OrderId);
                var order = new PurchaseOrderAggreagate(stream.Events);
                
                try
                {
                    
                    order.AddProduct(cmd.Qtd, cmd.ProductId, _productService);
                    _eventStore.AppendToStream(cmd.OrderId, stream.Version, order.Changes);
                    return;
                }
                catch (EventStoreConcurrencyException)
                {
                    
                }
            }
        }
    }
}
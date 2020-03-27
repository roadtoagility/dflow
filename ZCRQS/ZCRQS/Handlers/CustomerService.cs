using Core.Shared;

namespace Program.Handlers
{
    public class CustomerService
    {
        private readonly IEventStore _eventStore;

        public CustomerService(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }   
    }
}
using System;
using Core.Shared;
using Core.Shared.Interfaces;

namespace Program.Handlers
{
    public class CustomerService
    {
        private readonly IEventStore<Guid> _eventStore;

        public CustomerService(IEventStore<Guid> eventStore)
        {
            _eventStore = eventStore;
        }   
    }
}
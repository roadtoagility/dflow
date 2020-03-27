using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Core.Shared;
using Program.Commands;
using Program.Entities;
using Program.Handlers;
using Xunit;

namespace Program
{
    public class Tests
    {
        [Fact]
        public void EventsAppendedToChanges()
        {
            var appendOnly = new AppendOnlyStore();
            var eventStore = new EventStore(appendOnly);
            var productService = new ProductService(eventStore);
            var customerService = new CustomerService(eventStore);
            var command = new OrderServiceCommandHandler(eventStore, productService, customerService);
            
            var product = new Product(Guid.NewGuid(), "Notebook", "Dell Inspiron 15000");
            
            
            command.Execute(new AddProductCommand(Guid.NewGuid(), Guid.NewGuid(), 2));
        }
    }

    
}
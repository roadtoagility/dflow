using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Core.Shared;
using Program.Aggregates;
using Program.Commands;
using Program.Entities;
using Program.Events;
using Program.Handlers;
using Xunit;

namespace Program
{
    public class Tests
    {
        [Fact]
        public void ShouldCreateNewAggregate()
        {
            var appendOnly = new MemoryAppendOnlyStore();
            var eventStore = new EventStore(appendOnly);
            var rootId = Guid.NewGuid();
            var root = ProductCatalogAggregate.CreateRoot(rootId);

            Assert.Equal(2, root.Changes.Count);
            Assert.Equal(rootId,root.Id);
        }
        
        [Fact]
        public void ShouldSaveStream()
        {
            var rootId = Guid.NewGuid();
            var appendOnly = new MemoryAppendOnlyStore();
            var eventStore = new EventStore(appendOnly);
            var root = ProductCatalogAggregate.CreateRoot(rootId);
            
            eventStore.AppendToStream(root.Id, 0, root.Changes);

            var stream = eventStore.LoadEventStream(rootId);
            Assert.Equal(1, stream.Version);
            Assert.Equal(typeof(ProductCatalogAggregateCreate), stream.Events.ElementAt(0).GetType());
            Assert.Equal(typeof(ProductCatalogAggregateCreated), stream.Events.ElementAt(1).GetType());
        }
        
        [Fact]
        public void ShouldAggregateLoadStream()
        {
            var rootId = Guid.NewGuid();
            var appendOnly = new MemoryAppendOnlyStore();
            var eventStore = new EventStore(appendOnly);
            var rootToSave = ProductCatalogAggregate.CreateRoot(rootId);
            
            eventStore.AppendToStream(rootToSave.Id, 1, rootToSave.Changes);

            var stream = eventStore.LoadEventStream(rootId);
            var root = new ProductCatalogAggregate(stream.Events);
            
            Assert.True(0 == root.Changes.Count);
            Assert.Equal(rootId, root.Id);
        }
        
        [Fact]
        public void ShouldAllEventsRegisteredAggregate()
        {
            var appendOnly = new MemoryAppendOnlyStore();
            var eventStore = new EventStore(appendOnly);
            var root = ProductCatalogAggregate.CreateRoot(Guid.NewGuid());

            Assert.Equal(2, root.Changes.Count);
            Assert.Equal(typeof(ProductCatalogAggregateCreate), root.Changes.ElementAt(0).GetType());
            Assert.Equal(typeof(ProductCatalogAggregateCreated), root.Changes.ElementAt(1).GetType());
        }
        
        [Fact]
        public void EventsAppendedToChanges()
        {
            var appendOnly = new MemoryAppendOnlyStore();
            var eventStore = new EventStore(appendOnly);
            var productService = new ProductService(eventStore);
            var customerService = new CustomerService(eventStore);
            var command = new OrderServiceCommandHandler(eventStore, productService, customerService);
            
            var product = new Product(Guid.NewGuid(), "Notebook", "Dell Inspiron 15000");
            
            
            command.Execute(new AddProductCommand(Guid.NewGuid(), Guid.NewGuid(), 2));
        }
    }

    
}
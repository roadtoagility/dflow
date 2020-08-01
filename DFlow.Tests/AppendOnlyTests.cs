using System;
using System.Collections.Generic;
using System.Linq;
using DFlow.Bus;
using DFlow.Example;
using DFlow.Example.Aggregates;
using DFlow.Example.Events;
using DFlow.Example.Views;
using DFlow.Interfaces;
using DFlow.Store;
using Xunit;

namespace DFlow.Tests
{
    public class AppendOnlyTests
    {
        [Fact]
        public void AppendOnlyShouldSave()
        {
            var rootId = Guid.NewGuid();
            var eventBus = new MemoryEventBus();
            var appendOnly = new MemoryAppendOnlyStore(eventBus);
            var eventStore = new EventStore(appendOnly);
            
            appendOnly.Append(rootId, "NyAggregateType", 1, new List<IEvent>()
            {
                new ProductCreated(Guid.NewGuid(), "test", "")
            });

            var stream = eventStore.LoadEventStream(rootId);
            
            Assert.True(eventStore.Any(rootId));
            Assert.True(stream.Version == 1);
            Assert.True(stream.Events.Any());
        }
        
        [Fact]
        public void AppendOnlyShouldPublish()
        {
            var rootId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var eventBus = new MemoryEventBus();
            var appendOnly = new MemoryAppendOnlyStore(eventBus);
            var eventStore = new EventStore(appendOnly);
            
            var productView = new ProductView();
            eventBus.Subscribe<ProductCreated>(productView);
            
            appendOnly.Append(rootId, "NyAggregateType", 1, new List<IEvent>()
            {
                new ProductCreated(productId, "test", "")
            });

            Assert.True(productView.Products.Count == 1);
            Assert.True(productView.Products.ElementAt(0).Id == productId);
        }
    }
}
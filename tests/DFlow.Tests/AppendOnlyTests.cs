using System;
using System.Collections.Generic;
using System.Linq;
using DFlow.Base;
using DFlow.Bus;
using DFlow.Example;
using DFlow.Example.Aggregates;
using DFlow.Example.Events;
using DFlow.Example.Views;
using DFlow.Interfaces;
using Xunit;

namespace DFlow.Tests
{
    public class AppendOnlyTests
    {
        [Fact]
        public void AppendOnlyShouldSave()
        {
            var rootId = Guid.NewGuid();
            var eventBus = new MemoryEventBus(new MemoryResolver());
            var appendOnly = new MemoryAppendOnlyStore(eventBus);
            var eventStore = new EventStore(appendOnly, eventBus);
            
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
            var resolver = new MemoryResolver();
            var eventBus = new MemoryEventBus(resolver);
            var appendOnly = new MemoryAppendOnlyStore(eventBus);
            var eventStore = new EventStore(appendOnly, eventBus);
            
            var productView = new ProductView();
            resolver.Register<ProductCreated>(productView);
            
            var events = new List<IEvent>()
            {
                new ProductCreated(productId, "test", "")
            };
            
            var domainEvents = new List<IDomainEvent>()
            {
                new ProductCreated(productId, "test", "")
            };
            
            eventStore.AppendToStream<Guid>(rootId, 0, events, domainEvents.ToArray());
            
            Assert.True(productView.Products.Count == 1);
            Assert.True(productView.Products.ElementAt(0).Id == productId);
        }
    }
}
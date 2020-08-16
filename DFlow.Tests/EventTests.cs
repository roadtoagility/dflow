using System;
using DFlow.Base.Exceptions;
using DFlow.Bus;
using DFlow.Example;
using DFlow.Example.Aggregates;
using DFlow.Example.Commands;
using Xunit;

namespace DFlow.Tests
{
    public class EventTests
    {
        [Fact]
        public void ShouldCreateAggregateWithVersionCorrectly()
        {
            var eventBus = new MemoryEventBus(new MemoryResolver());
            var appendOnly = new MemoryAppendOnlyStore(eventBus);
            var eventStore = new EventStore(appendOnly, eventBus);
            var rootId = Guid.NewGuid();
            var factory = new AggregateFactory(eventStore);
            var root = factory.Create<ProductCatalogAggregate>(rootId);
            
            Assert.True(1 == root.Version);
        }
        
        [Fact]
        public void ShouldIncreaseVersionCorrectly()
        {
            var eventBus = new MemoryEventBus(new MemoryResolver());
            var appendOnly = new MemoryAppendOnlyStore(eventBus);
            var eventStore = new EventStore(appendOnly, eventBus);
            var rootId = Guid.NewGuid();
            var factory = new AggregateFactory(eventStore);
            var root = factory.Create<ProductCatalogAggregate>(rootId);
            eventStore.AppendToStream<ProductCatalogAggregate>(root.Id, root.Version, root.Changes);
            
            root = factory.Load<ProductCatalogAggregate>(rootId);
            root.CreateProduct(new CreateProductCommand(rootId,Guid.NewGuid(), "Notebook", "Dell Inspiron 15000"));
            root.CreateProduct(new CreateProductCommand(rootId,Guid.NewGuid(), "Notebook Asus Vivobook", "Notebook Asus Vivobook X441B-CBA6A de 14 Con AMD A6-9225/4GB Ram/500GB HD/W10"));
            root.CreateProduct(new CreateProductCommand(rootId,Guid.NewGuid(), "Notebook 2 em 1 Dell", "Notebook 2 em 1 Dell Inspiron i14-5481-M11F 8ª Geração Intel Core i3 4GB 128GB SSD 14' Touch Windows 10 Office 365 McAfe"));
            eventStore.AppendToStream<ProductCatalogAggregate>(root.Id, root.Version, root.Changes);
            
            root = factory.Load<ProductCatalogAggregate>(rootId);
            
            Assert.True(4 == root.Version);
        }
        
        [Fact]
        public void ShouldNotAllowCreateAggregatesWithSameId()
        {
            var eventBus = new MemoryEventBus(new MemoryResolver());
            var appendOnly = new MemoryAppendOnlyStore(eventBus);
            var eventStore = new EventStore(appendOnly, eventBus);
            var rootId = Guid.NewGuid();
            var factory = new AggregateFactory(eventStore);
            
            var root = factory.Create<ProductCatalogAggregate>(rootId);
            eventStore.AppendToStream<ProductCatalogAggregate>(root.Id, root.Version, root.Changes);
            
            Assert.Throws<DuplicatedRootException>(() 
                => factory.Create<ProductCatalogAggregate>(rootId)
            );
        }
    }
}
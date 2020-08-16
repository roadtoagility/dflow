using System;
using System.Linq;
using DFlow.Base.Aggregate;
using DFlow.Bus;
using DFlow.Example;
using DFlow.Example.Aggregates;
using DFlow.Example.Commands;
using DFlow.Example.Events;
using DFlow.Example.Views;
using DFlow.Store;
using Xunit;

namespace DFlow.Tests
{
    public class BasicTests
    {
        [Fact]
        public void ShouldCreateNewAggregate()
        {
            var eventBus = new MemoryEventBus();
            var appendOnly = new MemoryAppendOnlyStore(eventBus);
            var eventStore = new EventStore(appendOnly, eventBus);
            var rootId = Guid.NewGuid();
            var factory = new AggregateFactory(eventStore);
            var root = factory.Create<ProductCatalogAggregate>(rootId);

            Assert.Equal(rootId,root.Id);
            Assert.True(1 == root.Changes.Count);
            Assert.Equal(typeof(AggregateCreated<Guid>), root.Changes.ElementAt(0).GetType());
        }
        
        [Fact]
        public void ShouldSaveStream()
        {
            var rootId = Guid.NewGuid();
            var eventBus = new MemoryEventBus();
            var appendOnly = new MemoryAppendOnlyStore(eventBus);
            var eventStore = new EventStore(appendOnly, eventBus);
            var factory = new AggregateFactory(eventStore);
            var root = factory.Create<ProductCatalogAggregate>(rootId);
            
            eventStore.AppendToStream<ProductCatalogAggregate>(root.Id, root.Version, root.Changes);

            var stream = eventStore.LoadEventStream(rootId);
            Assert.Equal(1, stream.Version);
            Assert.Equal(typeof(AggregateCreated<Guid>), stream.Events.ElementAt(0).GetType());
        }
        
        [Fact]
        public void ShouldAggregateLoadStream()
        {
            var rootId = Guid.NewGuid();
            var eventBus = new MemoryEventBus();
            var appendOnly = new MemoryAppendOnlyStore(eventBus);
            var eventStore = new EventStore(appendOnly, eventBus);
            var factory = new AggregateFactory(eventStore);
            
            var rootToSave = factory.Create<ProductCatalogAggregate>(rootId);
            
            eventStore.AppendToStream<ProductCatalogAggregate>(rootToSave.Id, 1, rootToSave.Changes);
            
            var root = factory.Load<ProductCatalogAggregate>(rootId);
            
            Assert.True(0 == root.Changes.Count);
            Assert.Equal(rootId, root.Id);
        }
        
        //TODO: esse teste está sem objetivo claro
        [Fact]
        public void ShouldAllEventsRegisteredAggregate()
        {
            var eventBus = new MemoryEventBus();
            var appendOnly = new MemoryAppendOnlyStore(eventBus);
            var eventStore = new EventStore(appendOnly, eventBus);
            var factory = new AggregateFactory(eventStore);
            var root = factory.Create<ProductCatalogAggregate>(Guid.NewGuid());

            Assert.True(1 == root.Changes.Count);
            Assert.Equal(typeof(AggregateCreated<Guid>), root.Changes.ElementAt(0).GetType());
        }
        
        [Fact]
        public void ShouldAddProductToProductCatalog()
        {
            var rootId = Guid.NewGuid();
            var eventBus = new MemoryEventBus();
            var appendOnly = new MemoryAppendOnlyStore(eventBus);
            var eventStore = new EventStore(appendOnly, eventBus);
            var factory = new AggregateFactory(eventStore);
            var rootToSave = factory.Create<ProductCatalogAggregate>(rootId);
            
            eventStore.AppendToStream<ProductCatalogAggregate>(rootToSave.Id, rootToSave.Version, rootToSave.Changes);

            var root = factory.Load<ProductCatalogAggregate>(rootId);
            
            root.CreateProduct(new CreateProductCommand(rootId, Guid.NewGuid(), "Notebook", "Dell Inspiron 15000"));
            
            Assert.True(root.Changes.Count == 1);
            Assert.True(root.Changes.ElementAt(0).GetType() == typeof(ProductCreated));
        }
        
        [Fact]
        public void ShouldIncrementVersionCorrectly()
        {
            var rootId = Guid.NewGuid();
            var eventBus = new MemoryEventBus();
            var appendOnly = new MemoryAppendOnlyStore(eventBus);
            var eventStore = new EventStore(appendOnly, eventBus);
            var factory = new AggregateFactory(eventStore);
            var rootToSave = factory.Create<ProductCatalogAggregate>(rootId);
            
            eventStore.AppendToStream<ProductCatalogAggregate>(rootToSave.Id, rootToSave.Version, rootToSave.Changes);

            var stream = eventStore.LoadEventStream(rootId);
            var root = new ProductCatalogAggregate(stream);
            
            root.CreateProduct(new CreateProductCommand(rootId, Guid.NewGuid(), "Notebook", "Dell Inspiron 15000"));
            
            eventStore.AppendToStream<ProductCatalogAggregate>(root.Id, root.Version, root.Changes);

            stream = eventStore.LoadEventStream(rootId);
            root = new ProductCatalogAggregate(stream);
            
            Assert.Equal(2, stream.Version);
            Assert.Equal(1, root.CountProducts());
        }
        
        [Fact]
        public void ShouldUpdateProductProjection()
        {
            var rootId = Guid.NewGuid();
            var eventBus = new MemoryEventBus();
            var appendOnly = new MemoryAppendOnlyStore(eventBus);
            var eventStore = new EventStore(appendOnly, eventBus);
            var view = new ProductView();
            
            eventBus.Subscribe<ProductCreated>(view);
            
            var factory = new AggregateFactory(eventStore);
            var rootToSave = factory.Create<ProductCatalogAggregate>(rootId);
            
            eventStore.AppendToStream<ProductCatalogAggregate>(rootToSave.Id, 1, rootToSave.Changes);

            var stream = eventStore.LoadEventStream(rootId);
            var root = new ProductCatalogAggregate(stream);
            
            root.CreateProduct(new CreateProductCommand(rootId, Guid.NewGuid(), "Notebook", "Dell Inspiron 15000"));
            
            eventStore.AppendToStream<ProductCatalogAggregate>(root.Id, root.Version, root.Changes);
            
            Assert.True(1 == view.Products.Count);
            Assert.Equal("Notebook", view.Products[0].Name);
        }
    }
}
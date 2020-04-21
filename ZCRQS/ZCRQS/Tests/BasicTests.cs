using System;
using System.Linq;
using Core.Infrastructure.Queue.Memory;
using Core.Infrastructure.Write.Memory;
using Core.Shared;
using Core.Shared.Base;
using Core.Shared.Base.Aggregate;
using Program.Aggregates;
using Program.Commands;
using Program.Entities;
using Program.Events;
using Program.Handlers;
using Program.Views;
using Xunit;

namespace Program.Tests
{
    public class BasicTests
    {
        [Fact]
        public void ShouldCreateNewAggregate()
        {
            var queueService = new MemoryQueueService();
            var appendOnly = new MemoryAppendOnlyStore(queueService);
            var eventStore = new EventStore(appendOnly, queueService);
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
            var queueService = new MemoryQueueService();
            var appendOnly = new MemoryAppendOnlyStore(queueService);
            var eventStore = new EventStore(appendOnly, queueService);
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
            var queueService = new MemoryQueueService();
            var appendOnly = new MemoryAppendOnlyStore(queueService);
            var eventStore = new EventStore(appendOnly, queueService);
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
            var queueService = new MemoryQueueService();
            var appendOnly = new MemoryAppendOnlyStore(queueService);
            var eventStore = new EventStore(appendOnly, queueService);
            var factory = new AggregateFactory(eventStore);
            var root = factory.Create<ProductCatalogAggregate>(Guid.NewGuid());

            Assert.True(1 == root.Changes.Count);
            Assert.Equal(typeof(AggregateCreated<Guid>), root.Changes.ElementAt(0).GetType());
        }
        
        [Fact]
        public void ShouldAddProductToProductCatalog()
        {
            var rootId = Guid.NewGuid();
            var queueService = new MemoryQueueService();
            var appendOnly = new MemoryAppendOnlyStore(queueService);
            var eventStore = new EventStore(appendOnly, queueService);
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
            var queueService = new MemoryQueueService();
            var appendOnly = new MemoryAppendOnlyStore(queueService);
            var eventStore = new EventStore(appendOnly, queueService);
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
            var queueService = new MemoryQueueService();
            var appendOnly = new MemoryAppendOnlyStore(queueService);
            var eventStore = new EventStore(appendOnly, queueService);
            var view = new ProductView();
            
            queueService.Subscribe<ProductCreated>(view);
            
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
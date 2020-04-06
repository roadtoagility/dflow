using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Infrastructure.Queue.Memory;
using Core.Infrastructure.Write.Memory;
using Core.Shared.Base.Aggregate;
using Core.Shared.Base.Exceptions;
using Program.Aggregates;
using Program.Commands;
using Program.Events;
using Program.Handlers;
using Xunit;

namespace Program.Tests
{
    public class ConcurrencyTests
    {
        [Fact]
        public void ShouldMergeEvents()
        {
            var appendOnly = new MemoryAppendOnlyStore();
            var queueService = new MemoryQueueService();
            var eventStore = new EventStore(appendOnly, queueService);
            var snapShotRepo = new SnapshotRepository();
            var factory = new AggregateFactory(eventStore, snapShotRepo);
            
            var rootId = Guid.NewGuid();
            var handler = new ProductServiceCommandHandler(eventStore, factory);
            
            handler.Execute(new CreateProductCatalog(rootId));
            var prodId = Guid.NewGuid();
            handler.Execute(new CreateProductCommand(rootId, prodId, "Notebook Lenovo 2 em 1 ideapad C340", "Notebook Lenovo 2 em 1 ideapad C340 i7-8565U 8GB 256GB SSD Win10 14' FHD IPS - 81RL0001BR"));

            var threads = new List<Task>();
            
            threads.Add(Task.Run(() =>
            {
                var handlerThread1 = new ProductServiceCommandHandler(eventStore, factory);
                handlerThread1.Execute(new CreateProductCommand(rootId,Guid.NewGuid(), "Notebook 2 em 1 Dell", "Notebook 2 em 1 Dell Inspiron i14-5481-M11F 8ª Geração Intel Core i3 4GB 128GB SSD 14' Touch Windows 10 Office 365 McAfe"));
            }));
            
            threads.Add(Task.Run(() =>
            {
                var handlerThread2 = new ProductServiceCommandHandler(eventStore, factory);
                handlerThread2.Execute(new ChangeProductNameCommand(rootId, prodId, "Novo nome"));
            }));

            Task.WaitAll(threads.ToArray());

            var stream = eventStore.LoadEventStream(rootId);
            var productChanged = (ProductNameChanged)stream.Events.Where(x => x.GetType() == typeof(ProductNameChanged)).FirstOrDefault();
            
            Assert.True(4 == stream.Version);
            Assert.True("Novo nome" == productChanged.Name);
        }
        
        [Fact]
        public void ShouldThrowExceptionConflictEvents()
        {
            var rootId = Guid.NewGuid();
            var appendOnly = new MemoryAppendOnlyStore();
            var queueService = new MemoryQueueService();
            var eventStore = new EventStore(appendOnly, queueService);
            var snapShotRepo = new SnapshotRepository();
            var factory = new AggregateFactory(eventStore, snapShotRepo);
            
            var productAggregate = factory.Create<ProductCatalogAggregate>(rootId);
            productAggregate.CreateProduct(new CreateProductCommand(rootId, Guid.NewGuid(), "Notebook Acer Aspire 3", "Notebook Acer Aspire 3 A315-53-348W Intel Core i3-6006U  RAM de 4GB HD de 1TB Tela de 15.6” HD Windows 10"));
            eventStore.AppendToStream<ProductCatalogAggregate>(productAggregate.Id, productAggregate.Version, productAggregate.Changes);

            var user1 = factory.Load<ProductCatalogAggregate>(rootId);
            var user2 = factory.Load<ProductCatalogAggregate>(rootId);
            
            
            user1.CreateProduct(new CreateProductCommand(rootId,Guid.NewGuid(), "Notebook Asus Vivobook", "Notebook Asus Vivobook X441B-CBA6A de 14 Con AMD A6-9225/4GB Ram/500GB HD/W10"));
            eventStore.AppendToStream<ProductCatalogAggregate>(user1.Id, user1.Version, user1.Changes);
            
            user2.CreateProduct(new CreateProductCommand(rootId,Guid.NewGuid(), "Notebook 2 em 1 Dell", "Notebook 2 em 1 Dell Inspiron i14-5481-M11F 8ª Geração Intel Core i3 4GB 128GB SSD 14' Touch Windows 10 Office 365 McAfe"));
            
            Assert.Throws<EventStoreConcurrencyException>(() 
                => eventStore.AppendToStream<ProductCatalogAggregate>(user2.Id, user2.Version, user2.Changes)
            );
        }
    }
}
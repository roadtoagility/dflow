using System;
using System.Collections.Generic;
using Core.Shared;
using Core.Shared.Base;
using Core.Shared.Base.Aggregate;
using Program.Aggregates;
using Program.Commands;
using Xunit;
using Xunit.Sdk;

namespace Program.Tests
{
    public class SnapshotTests
    {
        [Fact]
        public void RetrieveAggregateLoadedFromSnapshot()
        {
            var rootId = Guid.NewGuid();
            var appendOnly = new MemoryAppendOnlyStore();
            var queueService = new MemoryQueueService();
            var eventStore = new EventStore(appendOnly, queueService);
            var snapShotRepo = new SnapshotRepository();
            var factory = new AggregateFactory(eventStore, snapShotRepo);
            
            var productAggregate = factory.Create<ProductCatalogAggregate>(rootId);
            
            productAggregate.CreateProduct(new CreateProductCommand(Guid.NewGuid(), "Notebook Acer Aspire 3", "Notebook Acer Aspire 3 A315-53-348W Intel Core i3-6006U  RAM de 4GB HD de 1TB Tela de 15.6” HD Windows 10"));
            productAggregate.CreateProduct(new CreateProductCommand(Guid.NewGuid(), "Notebook Asus Vivobook", "Notebook Asus Vivobook X441B-CBA6A de 14 Con AMD A6-9225/4GB Ram/500GB HD/W10"));
            productAggregate.CreateProduct(new CreateProductCommand(Guid.NewGuid(), "Notebook 2 em 1 Dell", "Notebook 2 em 1 Dell Inspiron i14-5481-M11F 8ª Geração Intel Core i3 4GB 128GB SSD 14' Touch Windows 10 Office 365 McAfe"));
            productAggregate.CreateProduct(new CreateProductCommand(Guid.NewGuid(), "Notebook Gamer Dell", "Notebook Gamer Dell G3-3590-M60P 9ª Geração Intel Core i7 8GB 512GB SSD Placa Vídeo NVIDIA 1660Ti 15.6' Windows 10"));
            productAggregate.CreateProduct(new CreateProductCommand(Guid.NewGuid(), "Notebook Lenovo 2 em 1 ideapad C340", "Notebook Lenovo 2 em 1 ideapad C340 i7-8565U 8GB 256GB SSD Win10 14' FHD IPS - 81RL0001BR"));
            
            eventStore.AppendToStream<ProductCatalogAggregate>(productAggregate.Id, 0, productAggregate.Changes);

            var stream = eventStore.LoadEventStream(rootId);
            snapShotRepo.SaveSnapshot(rootId, productAggregate, stream.Version);

            var aggregateReloadedFromSnapshot = factory.Load<ProductCatalogAggregate>(rootId);
            
            ProductCatalogAggregate root;
            long snapshotVersion = 0;
            var snap = snapShotRepo.TryGetSnapshotById(rootId, out root, out snapshotVersion);
            stream = eventStore.LoadEventStreamAfterVersion(rootId, snapshotVersion);
            
            Assert.True(snap);
            Assert.True(root != null);
            Assert.True(snapshotVersion == 1);
            Assert.True(stream.Events.Count == 0);
            Assert.True(aggregateReloadedFromSnapshot.Changes.Count == 0);
            Assert.True(aggregateReloadedFromSnapshot.Id == rootId);
            Assert.True(aggregateReloadedFromSnapshot.CountProducts() == 5);
        }

        [Fact]
        public void ShouldApplyEventsAfterSnapshot()
        {
            var rootId = Guid.NewGuid();
            var appendOnly = new MemoryAppendOnlyStore();
            var queueService = new MemoryQueueService();
            var eventStore = new EventStore(appendOnly, queueService);
            var snapShotRepo = new SnapshotRepository();
            var factory = new AggregateFactory(eventStore, snapShotRepo);
            
            var productAggregate = factory.Create<ProductCatalogAggregate>(rootId);
            
            productAggregate.CreateProduct(new CreateProductCommand(Guid.NewGuid(), "Notebook Acer Aspire 3", "Notebook Acer Aspire 3 A315-53-348W Intel Core i3-6006U  RAM de 4GB HD de 1TB Tela de 15.6” HD Windows 10"));
            productAggregate.CreateProduct(new CreateProductCommand(Guid.NewGuid(), "Notebook Asus Vivobook", "Notebook Asus Vivobook X441B-CBA6A de 14 Con AMD A6-9225/4GB Ram/500GB HD/W10"));
            eventStore.AppendToStream<ProductCatalogAggregate>(productAggregate.Id, 0, productAggregate.Changes);
            var stream = eventStore.LoadEventStream(rootId);
            snapShotRepo.SaveSnapshot(rootId, productAggregate, stream.Version);
            

            productAggregate = factory.Load<ProductCatalogAggregate>(rootId);
            productAggregate.CreateProduct(new CreateProductCommand(Guid.NewGuid(), "Notebook 2 em 1 Dell", "Notebook 2 em 1 Dell Inspiron i14-5481-M11F 8ª Geração Intel Core i3 4GB 128GB SSD 14' Touch Windows 10 Office 365 McAfe"));
            productAggregate.CreateProduct(new CreateProductCommand(Guid.NewGuid(), "Notebook Gamer Dell", "Notebook Gamer Dell G3-3590-M60P 9ª Geração Intel Core i7 8GB 512GB SSD Placa Vídeo NVIDIA 1660Ti 15.6' Windows 10"));
            productAggregate.CreateProduct(new CreateProductCommand(Guid.NewGuid(), "Notebook Lenovo 2 em 1 ideapad C340", "Notebook Lenovo 2 em 1 ideapad C340 i7-8565U 8GB 256GB SSD Win10 14' FHD IPS - 81RL0001BR"));
            //TODO: preciso ver o controle de versão, ta muito manual e não tenho certeza se o objeto que vai solicitar o AppToStream deve eviar a versão que ele espera que seja a current
            eventStore.AppendToStream<ProductCatalogAggregate>(productAggregate.Id, 1, productAggregate.Changes);
            
            
            ProductCatalogAggregate root;
            long snapshotVersion = 0;
            var snap = snapShotRepo.TryGetSnapshotById(rootId, out root, out snapshotVersion);
            stream = eventStore.LoadEventStreamAfterVersion(rootId, snapshotVersion);
            var aggregateReloadedFromSnapshot = factory.Load<ProductCatalogAggregate>(rootId);
            
            Assert.True(snap);
            Assert.True(root != null);
            Assert.True(snapshotVersion == 1);
            Assert.True(stream.Events.Count == 3);
            Assert.True(aggregateReloadedFromSnapshot.Changes.Count == 0);
            Assert.True(aggregateReloadedFromSnapshot.Id == rootId);
            Assert.True(aggregateReloadedFromSnapshot.CountProducts() == 5);
        }
        
        [Fact]
        public void ShouldCreateMultiplesSnapshots()
        {
            var rootId = Guid.NewGuid();
            var appendOnly = new MemoryAppendOnlyStore();
            var queueService = new MemoryQueueService();
            var eventStore = new EventStore(appendOnly, queueService);
            var snapShotRepo = new SnapshotRepository();
            var factory = new AggregateFactory(eventStore, snapShotRepo);
            
            var productAggregate = factory.Create<ProductCatalogAggregate>(rootId);
            productAggregate.CreateProduct(new CreateProductCommand(Guid.NewGuid(), "Notebook Acer Aspire 3", "Notebook Acer Aspire 3 A315-53-348W Intel Core i3-6006U  RAM de 4GB HD de 1TB Tela de 15.6” HD Windows 10"));
            eventStore.AppendToStream<ProductCatalogAggregate>(productAggregate.Id, 0, productAggregate.Changes);
            var stream = eventStore.LoadEventStream(rootId);
            snapShotRepo.SaveSnapshot(rootId, productAggregate, stream.Version);
            
            productAggregate = factory.Load<ProductCatalogAggregate>(rootId);
            productAggregate.CreateProduct(new CreateProductCommand(Guid.NewGuid(), "Notebook Asus Vivobook", "Notebook Asus Vivobook X441B-CBA6A de 14 Con AMD A6-9225/4GB Ram/500GB HD/W10"));
            eventStore.AppendToStream<ProductCatalogAggregate>(productAggregate.Id, 1, productAggregate.Changes);
            stream = eventStore.LoadEventStream(rootId);
            snapShotRepo.SaveSnapshot(rootId, productAggregate, stream.Version);
            
            productAggregate = factory.Load<ProductCatalogAggregate>(rootId);
            productAggregate.CreateProduct(new CreateProductCommand(Guid.NewGuid(), "Notebook 2 em 1 Dell", "Notebook 2 em 1 Dell Inspiron i14-5481-M11F 8ª Geração Intel Core i3 4GB 128GB SSD 14' Touch Windows 10 Office 365 McAfe"));
            eventStore.AppendToStream<ProductCatalogAggregate>(productAggregate.Id, 2, productAggregate.Changes);
            stream = eventStore.LoadEventStream(rootId);
            snapShotRepo.SaveSnapshot(rootId, productAggregate, stream.Version);
            
            productAggregate = factory.Load<ProductCatalogAggregate>(rootId);
            productAggregate.CreateProduct(new CreateProductCommand(Guid.NewGuid(), "Notebook Gamer Dell", "Notebook Gamer Dell G3-3590-M60P 9ª Geração Intel Core i7 8GB 512GB SSD Placa Vídeo NVIDIA 1660Ti 15.6' Windows 10"));
            eventStore.AppendToStream<ProductCatalogAggregate>(productAggregate.Id, 3, productAggregate.Changes);
            stream = eventStore.LoadEventStream(rootId);
            snapShotRepo.SaveSnapshot(rootId, productAggregate, stream.Version);
            
            productAggregate = factory.Load<ProductCatalogAggregate>(rootId);
            productAggregate.CreateProduct(new CreateProductCommand(Guid.NewGuid(), "Notebook Lenovo 2 em 1 ideapad C340", "Notebook Lenovo 2 em 1 ideapad C340 i7-8565U 8GB 256GB SSD Win10 14' FHD IPS - 81RL0001BR"));
            eventStore.AppendToStream<ProductCatalogAggregate>(productAggregate.Id, 4, productAggregate.Changes);
            stream = eventStore.LoadEventStream(rootId);
            snapShotRepo.SaveSnapshot(rootId, productAggregate, stream.Version);
            
            var aggregateReloadedFromSnapshot = factory.Load<ProductCatalogAggregate>(rootId);
            
            ProductCatalogAggregate root;
            long snapshotVersion = 0;
            var snap = snapShotRepo.TryGetSnapshotById(rootId, out root, out snapshotVersion);
            stream = eventStore.LoadEventStreamAfterVersion(rootId, snapshotVersion);
            
            Assert.True(snap);
            Assert.True(root != null);
            Assert.True(snapshotVersion == 5);
            Assert.True(stream.Events.Count == 0);
            Assert.True(aggregateReloadedFromSnapshot.Changes.Count == 0);
            Assert.True(aggregateReloadedFromSnapshot.Id == rootId);
            Assert.True(aggregateReloadedFromSnapshot.CountProducts() == 5);
        }
    }

    
}
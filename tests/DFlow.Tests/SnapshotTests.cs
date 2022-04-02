using System;
using System.Linq;
using DFlow.Base;
using DFlow.Bus;
using DFlow.Example;
using DFlow.Example.Aggregates;
using DFlow.Example.Commands;
using DFlow.Example.Events;
using DFlow.Example.Views;
using Xunit;

namespace DFlow.Tests
{
    public class SnapshotTests
    {
        [Fact]
        public void RetrieveAggregateLoadedFromSnapshot()
        {
            var rootId = Guid.NewGuid();
            var resolver = new MemoryResolver();
            var eventBus = new MemoryEventBus(resolver);
            var appendOnly = new MemoryAppendOnlyStore(eventBus);
            var eventStore = new EventStore(appendOnly, eventBus);
            var snapShotRepo = new SnapshotRepository();
            var factory = new AggregateFactory(eventStore, snapShotRepo);

            var view = new ProductView();
            resolver.Register<ProductCreated>(view);

            var productAggregate = factory.Create<ProductCatalogAggregate>(rootId);

            productAggregate.CreateProduct(new CreateProductCommand(rootId, Guid.NewGuid(), "Notebook Acer Aspire 3",
                "Notebook Acer Aspire 3 A315-53-348W Intel Core i3-6006U  RAM de 4GB HD de 1TB Tela de 15.6” HD Windows 10"));
            productAggregate.CreateProduct(new CreateProductCommand(rootId, Guid.NewGuid(), "Notebook Asus Vivobook",
                "Notebook Asus Vivobook X441B-CBA6A de 14 Con AMD A6-9225/4GB Ram/500GB HD/W10"));
            productAggregate.CreateProduct(new CreateProductCommand(rootId, Guid.NewGuid(), "Notebook 2 em 1 Dell",
                "Notebook 2 em 1 Dell Inspiron i14-5481-M11F 8ª Geração Intel Core i3 4GB 128GB SSD 14' Touch Windows 10 Office 365 McAfe"));
            productAggregate.CreateProduct(new CreateProductCommand(rootId, Guid.NewGuid(), "Notebook Gamer Dell",
                "Notebook Gamer Dell G3-3590-M60P 9ª Geração Intel Core i7 8GB 512GB SSD Placa Vídeo NVIDIA 1660Ti 15.6' Windows 10"));
            productAggregate.CreateProduct(new CreateProductCommand(rootId, Guid.NewGuid(),
                "Notebook Lenovo 2 em 1 ideapad C340",
                "Notebook Lenovo 2 em 1 ideapad C340 i7-8565U 8GB 256GB SSD Win10 14' FHD IPS - 81RL0001BR"));

            eventStore.AppendToStream<ProductCatalogAggregate>(productAggregate.Id, productAggregate.Version,
                productAggregate.Changes, productAggregate.DomainEvents.ToArray());

            var stream = eventStore.LoadEventStream(rootId);
            snapShotRepo.SaveSnapshot(rootId, productAggregate, stream.Version);

            var aggregateReloadedFromSnapshot = factory.Load<ProductCatalogAggregate>(rootId);

            ProductCatalogAggregate root;
            long snapshotVersion = 0;
            var snap = snapShotRepo.TryGetSnapshotById(rootId, out root, out snapshotVersion);
            stream = eventStore.LoadEventStreamAfterVersion(rootId, snapshotVersion);

            Assert.True(snap);
            Assert.True(root != null);
            Assert.True(snapshotVersion == 6);
            Assert.True(stream.Events.Count == 0);
            Assert.True(aggregateReloadedFromSnapshot.Changes.Count == 0);
            Assert.True(aggregateReloadedFromSnapshot.Id == rootId);
            Assert.True(view.Products.Count == 5);
        }

        [Fact]
        public void ShouldApplyEventsAfterSnapshot()
        {
            var rootId = Guid.NewGuid();
            var resolver = new MemoryResolver();
            var eventBus = new MemoryEventBus(resolver);
            var appendOnly = new MemoryAppendOnlyStore(eventBus);
            var eventStore = new EventStore(appendOnly, eventBus);
            var snapShotRepo = new SnapshotRepository();
            var factory = new AggregateFactory(eventStore, snapShotRepo);
            var view = new ProductView();
            resolver.Register<ProductCreated>(view);

            var productAggregate = factory.Create<ProductCatalogAggregate>(rootId);

            productAggregate.CreateProduct(new CreateProductCommand(rootId, Guid.NewGuid(), "Notebook Acer Aspire 3",
                "Notebook Acer Aspire 3 A315-53-348W Intel Core i3-6006U  RAM de 4GB HD de 1TB Tela de 15.6” HD Windows 10"));
            productAggregate.CreateProduct(new CreateProductCommand(rootId, Guid.NewGuid(), "Notebook Asus Vivobook",
                "Notebook Asus Vivobook X441B-CBA6A de 14 Con AMD A6-9225/4GB Ram/500GB HD/W10"));
            eventStore.AppendToStream<ProductCatalogAggregate>(productAggregate.Id, productAggregate.Version,
                productAggregate.Changes, productAggregate.DomainEvents.ToArray());
            var stream = eventStore.LoadEventStream(rootId);
            snapShotRepo.SaveSnapshot(rootId, productAggregate, stream.Version);


            productAggregate = factory.Load<ProductCatalogAggregate>(rootId);
            productAggregate.CreateProduct(new CreateProductCommand(rootId, Guid.NewGuid(), "Notebook 2 em 1 Dell",
                "Notebook 2 em 1 Dell Inspiron i14-5481-M11F 8ª Geração Intel Core i3 4GB 128GB SSD 14' Touch Windows 10 Office 365 McAfe"));
            productAggregate.CreateProduct(new CreateProductCommand(rootId, Guid.NewGuid(), "Notebook Gamer Dell",
                "Notebook Gamer Dell G3-3590-M60P 9ª Geração Intel Core i7 8GB 512GB SSD Placa Vídeo NVIDIA 1660Ti 15.6' Windows 10"));
            productAggregate.CreateProduct(new CreateProductCommand(rootId, Guid.NewGuid(),
                "Notebook Lenovo 2 em 1 ideapad C340",
                "Notebook Lenovo 2 em 1 ideapad C340 i7-8565U 8GB 256GB SSD Win10 14' FHD IPS - 81RL0001BR"));
            eventStore.AppendToStream<ProductCatalogAggregate>(productAggregate.Id, productAggregate.Version,
                productAggregate.Changes, productAggregate.DomainEvents.ToArray());


            ProductCatalogAggregate root;
            long snapshotVersion = 0;
            var snap = snapShotRepo.TryGetSnapshotById(rootId, out root, out snapshotVersion);
            stream = eventStore.LoadEventStreamAfterVersion(rootId, snapshotVersion);
            var aggregateReloadedFromSnapshot = factory.Load<ProductCatalogAggregate>(rootId);

            Assert.True(snap);
            Assert.True(root != null);
            Assert.True(3 == snapshotVersion);
            Assert.True(stream.Events.Count == 3);
            Assert.True(aggregateReloadedFromSnapshot.Changes.Count == 0);
            Assert.True(aggregateReloadedFromSnapshot.Id == rootId);
            Assert.True(view.Products.Count == 5);
        }

        [Fact]
        public void ShouldCreateMultiplesSnapshots()
        {
            var rootId = Guid.NewGuid();
            var resolver = new MemoryResolver();
            var eventBus = new MemoryEventBus(resolver);
            var appendOnly = new MemoryAppendOnlyStore(eventBus);
            var eventStore = new EventStore(appendOnly, eventBus);
            var snapShotRepo = new SnapshotRepository();
            var factory = new AggregateFactory(eventStore, snapShotRepo);

            var view = new ProductView();
            resolver.Register<ProductCreated>(view);


            var productAggregate = factory.Create<ProductCatalogAggregate>(rootId);
            productAggregate.CreateProduct(new CreateProductCommand(rootId, Guid.NewGuid(), "Notebook Acer Aspire 3",
                "Notebook Acer Aspire 3 A315-53-348W Intel Core i3-6006U  RAM de 4GB HD de 1TB Tela de 15.6” HD Windows 10"));
            eventStore.AppendToStream<ProductCatalogAggregate>(productAggregate.Id, productAggregate.Version,
                productAggregate.Changes, productAggregate.DomainEvents.ToArray());
            var stream = eventStore.LoadEventStream(rootId);
            snapShotRepo.SaveSnapshot(rootId, productAggregate, stream.Version);

            productAggregate = factory.Load<ProductCatalogAggregate>(rootId);
            productAggregate.CreateProduct(new CreateProductCommand(rootId, Guid.NewGuid(), "Notebook Asus Vivobook",
                "Notebook Asus Vivobook X441B-CBA6A de 14 Con AMD A6-9225/4GB Ram/500GB HD/W10"));
            eventStore.AppendToStream<ProductCatalogAggregate>(productAggregate.Id, productAggregate.Version,
                productAggregate.Changes, productAggregate.DomainEvents.ToArray());
            stream = eventStore.LoadEventStream(rootId);
            snapShotRepo.SaveSnapshot(rootId, productAggregate, stream.Version);

            productAggregate = factory.Load<ProductCatalogAggregate>(rootId);
            productAggregate.CreateProduct(new CreateProductCommand(rootId, Guid.NewGuid(), "Notebook 2 em 1 Dell",
                "Notebook 2 em 1 Dell Inspiron i14-5481-M11F 8ª Geração Intel Core i3 4GB 128GB SSD 14' Touch Windows 10 Office 365 McAfe"));
            eventStore.AppendToStream<ProductCatalogAggregate>(productAggregate.Id, productAggregate.Version,
                productAggregate.Changes, productAggregate.DomainEvents.ToArray());
            stream = eventStore.LoadEventStream(rootId);
            snapShotRepo.SaveSnapshot(rootId, productAggregate, stream.Version);

            productAggregate = factory.Load<ProductCatalogAggregate>(rootId);
            productAggregate.CreateProduct(new CreateProductCommand(rootId, Guid.NewGuid(), "Notebook Gamer Dell",
                "Notebook Gamer Dell G3-3590-M60P 9ª Geração Intel Core i7 8GB 512GB SSD Placa Vídeo NVIDIA 1660Ti 15.6' Windows 10"));
            eventStore.AppendToStream<ProductCatalogAggregate>(productAggregate.Id, productAggregate.Version,
                productAggregate.Changes, productAggregate.DomainEvents.ToArray());
            stream = eventStore.LoadEventStream(rootId);
            snapShotRepo.SaveSnapshot(rootId, productAggregate, stream.Version);

            productAggregate = factory.Load<ProductCatalogAggregate>(rootId);
            productAggregate.CreateProduct(new CreateProductCommand(rootId, Guid.NewGuid(),
                "Notebook Lenovo 2 em 1 ideapad C340",
                "Notebook Lenovo 2 em 1 ideapad C340 i7-8565U 8GB 256GB SSD Win10 14' FHD IPS - 81RL0001BR"));
            eventStore.AppendToStream<ProductCatalogAggregate>(productAggregate.Id, productAggregate.Version,
                productAggregate.Changes, productAggregate.DomainEvents.ToArray());
            stream = eventStore.LoadEventStream(rootId);
            snapShotRepo.SaveSnapshot(rootId, productAggregate, stream.Version);

            var aggregateReloadedFromSnapshot = factory.Load<ProductCatalogAggregate>(rootId);

            ProductCatalogAggregate root;
            long snapshotVersion = 0;
            var snap = snapShotRepo.TryGetSnapshotById(rootId, out root, out snapshotVersion);
            stream = eventStore.LoadEventStreamAfterVersion(rootId, snapshotVersion);

            Assert.True(snap);
            Assert.True(root != null);
            Assert.True(snapshotVersion == 6);
            Assert.True(stream.Events.Count == 0);
            Assert.True(aggregateReloadedFromSnapshot.Changes.Count == 0);
            Assert.True(aggregateReloadedFromSnapshot.Id == rootId);
            Assert.True(view.Products.Count == 5);
        }
    }
}
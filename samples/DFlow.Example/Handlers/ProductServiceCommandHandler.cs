using System;
using System.Linq;
using DFlow.Base;
using DFlow.Base.Events;
using DFlow.Base.Exceptions;
using DFlow.Example.Aggregates;
using DFlow.Example.Commands;
using DFlow.Interfaces;

namespace DFlow.Example.Handlers
{
    public class ProductServiceCommandHandler : Handler
    {
        private readonly IEventStore<Guid> _eventStore;
        private readonly AggregateFactory _factory;

        public ProductServiceCommandHandler(IEventStore<Guid> eventStore,
            AggregateFactory factory) : base(eventStore)
        {
            _eventStore = eventStore;
            _factory = factory;
        }

        public CommandEvent When(CreateProductCatalog cmd)
        {
            var productCatalog = _factory.Create<ProductCatalogAggregate>(cmd.Id);

            try
            {
                _eventStore.AppendToStream<ProductCatalogAggregate>(cmd.Id, productCatalog.Version,
                    productCatalog.Changes, productCatalog.DomainEvents.ToArray());

                return new CommandEvent(OperationStatus.Success);
            }
            catch (EventStoreConcurrencyException ex)
            {
                HandleConcurrencyException(ex, productCatalog);
                return new CommandEvent(OperationStatus.Success);
            }
        }

        public CommandEvent When(CreateProductCommand cmd)
        {
            var productCatalog = _factory.Load<ProductCatalogAggregate>(cmd.RootId);
            productCatalog.CreateProduct(cmd);

            try
            {
                _eventStore.AppendToStream<ProductCatalogAggregate>(cmd.RootId, productCatalog.Version,
                    productCatalog.Changes, productCatalog.DomainEvents.ToArray());

                //_publisher.Publish(arry<IDomainEvent> event)
                return new CommandEvent(OperationStatus.Success);
            }
            catch (EventStoreConcurrencyException ex)
            {
                HandleConcurrencyException(ex, productCatalog);
                return new CommandEvent(OperationStatus.Success);
            }
        }

        public CommandEvent When(ChangeProductNameCommand cmd)
        {
            var productCatalog = _factory.Load<ProductCatalogAggregate>(cmd.RootId);
            productCatalog.ChangeProductName(cmd);

            try
            {
                _eventStore.AppendToStream<ProductCatalogAggregate>(cmd.RootId, productCatalog.Version,
                    productCatalog.Changes, productCatalog.DomainEvents.ToArray());
                return new CommandEvent(OperationStatus.Success);
            }
            catch (EventStoreConcurrencyException ex)
            {
                HandleConcurrencyException(ex, productCatalog);
                return new CommandEvent(OperationStatus.Success);
            }
        }
    }
}
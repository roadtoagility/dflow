using System;
using System.Collections.Generic;
using Core.Shared;
using Core.Shared.Base;
using Core.Shared.Base.Aggregate;
using Core.Shared.Base.Exceptions;
using Core.Shared.Interfaces;
using Program.Aggregates;
using Program.Commands;
using Program.Events;

namespace Program.Handlers
{
    public class ProductServiceCommandHandler : Handler<IProductCatalogCommandHandler>
    {
        private readonly IEventStore<Guid> _eventStore;
        private readonly AggregateFactory _factory;

        public ProductServiceCommandHandler(IEventStore<Guid> eventStore,
            AggregateFactory factory) : base(eventStore)
        {
            _eventStore = eventStore;
            _factory = factory;
        }

        public Result<object> When(CreateProductCatalog cmd)
        {
            while(true)
            {
                var productCatalog = _factory.Create<ProductCatalogAggregate>(cmd.Id);
                
                try
                {
                    _eventStore.AppendToStream<ProductCatalogAggregate>(cmd.Id, productCatalog.Version,
                        productCatalog.Changes);
                    return new Result<object>(null, new List<Exception>());
                }
                catch (EventStoreConcurrencyException ex)
                {
                    HandleConcurrencyException(ex, productCatalog);
                    return new Result<object>(null, new List<Exception>(){ex});
                }
                catch(Exception)
                {
                    throw;
                }
            }
        }
        
        public Result<object> When(CreateProductCommand cmd)
        {
            while(true)
            {
                var productCatalog = _factory.Load<ProductCatalogAggregate>(cmd.RootId);
                productCatalog.CreateProduct(cmd);
                
                try
                {
                    _eventStore.AppendToStream<ProductCatalogAggregate>(cmd.RootId, productCatalog.Version,
                        productCatalog.Changes);
                    return new Result<object>(null, new List<Exception>());
                }
                catch (EventStoreConcurrencyException ex)
                {
                    HandleConcurrencyException(ex, productCatalog);
                }
                catch(Exception)
                {
                    throw;
                }
            }
        }
        
        public Result<object> When(ChangeProductNameCommand cmd)
        {
            while(true)
            {
                var productCatalog = _factory.Load<ProductCatalogAggregate>(cmd.RootId);
                productCatalog.ChangeProductName(cmd);
                
                try
                {
                    _eventStore.AppendToStream<ProductCatalogAggregate>(cmd.RootId, productCatalog.Version,
                        productCatalog.Changes);
                    return new Result<object>(null, new List<Exception>());
                }
                catch (EventStoreConcurrencyException ex)
                {
                    HandleConcurrencyException(ex, productCatalog);
                }
                catch(Exception)
                {
                    throw;
                }
            }
        }
    }
}
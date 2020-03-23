using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Core.Shared;

namespace ZCRQS
{
    public class Product : Entity<Guid>
    {
        public Guid Id { get; set; }
    }

    public interface Entity<T>
    {
        public T Id { get; set; }
    }
    
    #region Tools

    public interface IObserver
    {
        public void HandledEvent(IEvent @event);
        public string GetEventListenerName();
    }
    
    public interface IEventStore

    {

        // loads all events for a stream

        EventStream LoadEventStream(Guid id);

        // loads subset of events for a stream

        EventStream LoadEventStream(Guid id, int skipEvents, int maxCount);

        // appends events to a stream, throwing

        // OptimisticConcurrencyException another appended

        // new events since expectedversion

        void AppendToStream(Guid id, int expectedVersion, ICollection<IEvent> events);

    }

    public class EventStoreMessageBus
    {
        private List<IObserver> _observers = new List<IObserver>();
        private Dictionary<string, List<IObserver>> _observersDictionary = new Dictionary<string, List<IObserver>>();
        private IList<IEvent> _events;
        
        public EventStoreMessageBus()
        {
            
        }
        
        public void Subscribe(IObserver observer)
        {
            if (!_observersDictionary.ContainsKey(observer.GetEventListenerName()))
            {
                _observersDictionary.Add(observer.GetEventListenerName(), new List<IObserver>());
            }
                
            _observersDictionary[observer.GetEventListenerName()].Add(observer);
        }
        
        public void Publish(params IEvent[] events)
        {
            foreach (var @event in events)
            {
                if (_observersDictionary.ContainsKey(@event.GetEventName()))
                {
                    _events.Add(@event);
                    foreach (var observer in _observersDictionary[@event.GetEventName()])
                    {
                        observer.HandledEvent(@event);
                    }
                }
            }
        }

        public EventStreamResult LoadEventStream(Guid id)
        {
            return new EventStreamResult(){Events = _events.Where(x => x.GetRoot() == id).ToList(), Version = 0};
        }

        public void AppendToStream(Guid rootId, long version, List<IEvent> changes)
        {
            
        }
    }

    public class EventStreamResult
    {
        public List<IEvent> Events { get; set; }
        public long Version { get; set; }
    }

    public class EventStream
    {
        // version of the event stream returned

        public int Version;

        // all events in the stream

        public IList<IEvent> Events = new List<IEvent>();
    }
    
    public interface IAppendOnlyStore : IDisposable

    {

        void Append(string name, byte[] data, int expectedVersion = -1);

        IEnumerable<DataWithVersion> ReadRecords(

            string name, int afterVersion, int maxCount);

        IEnumerable<DataWithName> ReadRecords(

            int afterVersion, int maxCount);



        void Close();

    }
    
    public class DataWithVersion

    {

        public int Version;

        public byte[] Data;

    }
    
    public sealed class DataWithName

    {

        public string Name;

        public byte[] Data;

    }
    
    #endregion

    #region SERVICES
    public class ProductService
    {
        private readonly EventStoreMessageBus _eventStore;

        public ProductService(EventStoreMessageBus eventStore)
        {
            _eventStore = eventStore;
        }
    }

    public class OrderService
    {
        private readonly EventStoreMessageBus _eventStore;
        private readonly ProductService _productService;
        private readonly CustomerService _customerService;

        public OrderService(EventStoreMessageBus eventStore, ProductService productService, CustomerService customerService)
        {
            _eventStore = eventStore;
            _productService = productService;
            _customerService = customerService;
        }

        public void AddProductToOrder(Guid orderId, Guid productId, decimal qtd)
        {
            while(true)
            {
                var stream = _eventStore.LoadEventStream(orderId);
                var order = new PurchaseOrderAggreagate(stream.Events);
                
                try
                {
                    
                    order.AddProduct(qtd, productId, _productService);
                    _eventStore.AppendToStream(orderId, stream.Version, order.Changes);
                    return;
                }
                catch (EventStoreConcurrencyException)
                {
                    
                }
            }
        }
    }

    public class CustomerService
    {
        private readonly EventStoreMessageBus _eventStore;

        public CustomerService(EventStoreMessageBus eventStore)
        {
            _eventStore = eventStore;
        }   
    }
    #endregion
    
    #region EVENTS

    public class OrderCreatedEvent
    {
        
    }

    public class CreditReservedEvent
    {
        
    }

    public class CreditCheckFailedEvent
    {
        
    }

    public class EventStoreConcurrencyException : Exception
    {
        public List<IEvent> StoreEvents { get; set; }

        public long StoreVersion { get; set; }
    }

    #endregion
    
    #region AGGREGATES
    
    public class CustomerAggregate
    {
        
    }

    public class ProductCatalogAggregate
    {
        
    }
    
    
    public class PurchaseOrderAggreagate
    {
        public bool ConsumptionLocked { get; private set; }
        public List<IEvent> Changes { get; private set; }

        public PurchaseOrderAggreagate(IEnumerable<IEvent> events)
        {
            Changes = new List<IEvent>();
            
            foreach (var @event in events)
            {
                Mutate(@event);
            }
        }

        public void AddProduct(decimal qtd, Guid productId, ProductService productService)
        {
            
        }
        
        void Apply(IEvent @event)
        {
            Changes.Add(@event);
            Mutate(@event);
        }
        
        private void Mutate(IEvent e)
        {
            ((dynamic) this).When((dynamic)e);

        }
        
        private void When(PurchaseOrderLocked e)
        {

            ConsumptionLocked = true;

        }
        
        private void When(PurchaseOrderUnlocked e)
        {

            ConsumptionLocked = false;

        }
    }
    
    #endregion

  

    public class PurchaseOrderUnlocked
    {
    }

    public class PurchaseOrderLocked
    {
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Input;

namespace Program
{
    public interface IEvent
    {
        string GetEventName();
        string GetEntityType();

        Guid GetEventId();

        string GetEventType();

        string GetEventDate();
        
        Guid GetRoot();
    }
    
    
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

    public class EventStore : IEventStore
    {
        private Dictionary<string, List<IObserver>> _observersDictionary = new Dictionary<string, List<IObserver>>();
        private IList<IEvent> _events;
        readonly BinaryFormatter _formatter = new BinaryFormatter();
        readonly IAppendOnlyStore _appendOnlyStore;
        
        public EventStore(IAppendOnlyStore appendOnlyStore)
        {
            _appendOnlyStore = appendOnlyStore;
        }
        
        byte[] SerializeEvent(IEvent[] e)
        {
            using (var mem = new MemoryStream())
            {
                _formatter.Serialize(mem, e);
                return mem.ToArray();
            }
        }
        
        IEvent[] DeserializeEvent(byte[] data)
        {
            using (var mem = new MemoryStream(data))
            {
                return (IEvent[])_formatter.Deserialize(mem);
            }
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


        public EventStream LoadEventStream(Guid id, int skipEvents, int maxCount)
        {
            var name = id.ToString();

            var records = _appendOnlyStore.ReadRecords(name, skipEvents, maxCount).ToList();

            var stream = new EventStream();

            foreach (var tapeRecord in records)
            {
                stream.Events.AddRange(DeserializeEvent(tapeRecord.Data));
                stream.Version = tapeRecord.Version;
            }

            return stream;
        }

        public void AppendToStream(Guid id, int expectedVersion, ICollection<IEvent> events)
        {
            if (events.Count == 0)
                return;

            var name = id.ToString();
            var data = SerializeEvent(events.ToArray());

            try
            {
                _appendOnlyStore.Append(name, data, expectedVersion);
            }
            // catch(AppendOnlyStoreConcurrencyException e)
            catch(Exception ex)
            {
                // load server events
                var server = LoadEventStream(id, 0, int.MaxValue);

                throw;
                // throw a real problem
                // throw OptimisticConcurrencyException.Create(
                //
                //     server.Version, e.ExpectedVersion, id, server.Events);

            }
        }
        

        EventStream IEventStore.LoadEventStream(Guid id)
        {
            throw new NotImplementedException();
        }
    }

    public class EventStream
    {
        // version of the event stream returned

        public int Version;

        // all events in the stream

        public List<IEvent> Events = new List<IEvent>();
    }
    
    public interface IAppendOnlyStore : IDisposable
    {
        void Append(string name, byte[] data, int expectedVersion = -1);

        IEnumerable<DataWithVersion> ReadRecords(string name, int afterVersion, int maxCount);

        IEnumerable<DataWithName> ReadRecords(int afterVersion, int maxCount);

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
        private readonly IEventStore _eventStore;

        public ProductService(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }
    }
    
    public interface ISnapshotRepository
    {
        bool TryGetSnapshotById<TAggregate>(Guid id, out TAggregate snapshot, out int version);
        void SaveSnapshot<TAggregate>(Guid id, TAggregate snapshot, int version);
    }

    public interface IOrderServiceCommandHandler
    {
        void Execute(ICommand cmd);
    }
    
    public class OrderServiceCommandHandler : IOrderServiceCommandHandler
    {
        private readonly IEventStore _eventStore;
        private readonly ProductService _productService;
        private readonly CustomerService _customerService;

        public OrderServiceCommandHandler(IEventStore eventStore, ProductService productService, CustomerService customerService)
        {
            _eventStore = eventStore;
            _productService = productService;
            _customerService = customerService;
        }
        
        public void Execute(ICommand command)
        {
            ((dynamic)this).When((dynamic)command);
        }

        public void When(AddProductCommand cmd)
        {
            while(true)
            {
                var stream = _eventStore.LoadEventStream(cmd.OrderId);
                var order = new PurchaseOrderAggreagate(stream.Events);
                
                try
                {
                    
                    order.AddProduct(cmd.Qtd, cmd.ProductId, _productService);
                    _eventStore.AppendToStream(cmd.OrderId, stream.Version, order.Changes);
                    return;
                }
                catch (EventStoreConcurrencyException)
                {
                    
                }
            }
        }
    }

    public class AddProductCommand
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public decimal Qtd { get; set; }
    }

    public class CustomerService
    {
        private readonly IEventStore _eventStore;

        public CustomerService(IEventStore eventStore)
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
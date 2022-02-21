using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DFlow.Domain.BusinessObjects;
using DFlow.Persistence.EventStore.Model;
using DFlow.Persistence.EventStore.Repositories;

namespace DFlow.Persistence.EventStore.InMemoryStore
{
    public class EventStoreRepository : IEventStoreRepository<Guid>
    {
        
        readonly IAppendOnlyStore<Guid> _appendOnlyStoreRepository;
        readonly BinaryFormatter _formatter = new BinaryFormatter();
        
        public EventStoreRepository(IEventStoreRepository<Guid> appendOnlyStoreRepository)
        {
            _appendOnlyStoreRepository = appendOnlyStoreRepository;
        }
        
        public EventStream<Guid> LoadEventStream(Guid id) => LoadEventStream(id, 0, Int32.MaxValue);
        
        public EventStream<Guid> LoadEventStream(Guid id, int skipEvents, int maxCount)
        {
            var records = _appendOnlyStoreRepository.ReadRecords(id, skipEvents, maxCount).ToList();

            var stream = new EventStream();

            foreach (var tapeRecord in records)
            {
                stream.Events.AddRange(DeserializeEvent(tapeRecord.Data));
                stream.Version = tapeRecord.Version;
            }

            return stream;
        }

        public EventStream LoadEventStreamAfterVersion(Guid id, long afterVersion)
        {
            var records = _appendOnlyStoreRepository.ReadRecords(id, afterVersion, Int32.MaxValue).ToList();

            var stream = new EventStream();

            foreach (var tapeRecord in records)
            {
                stream.Events.AddRange(DeserializeEvent(tapeRecord.Data));
                stream.Version = tapeRecord.Version;
            }

            return stream;
        }

        public void AppendToStream<TType>(Guid id, long version, ICollection<IEvent> events, params IDomainEvent[] domainEvents)
        {
            //salvar os dados em uma lista interna
            var aggregateType = typeof(TType).Name;
            _appendOnlyStoreRepository.Append(id, aggregateType, version, events);
            _eventBus.Publish(domainEvents);
        }

        public bool Any(Guid id)
        {
            return _appendOnlyStoreRepository.Any(id);
        }
        
        IEvent[] DeserializeEvent(byte[] data)
        {
            using (var mem = new MemoryStream(data))
            {
                return (IEvent[])_formatter.Deserialize(mem);
            }
        }
    }
}
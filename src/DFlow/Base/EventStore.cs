using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using DFlow.Interfaces;

namespace DFlow.Base
{
    public class EventStore : IEventStore<Guid>
    {
        private readonly IAppendOnlyStore<Guid> _appendOnlyStore;
        private readonly IEventBus _eventBus;
        private readonly BinaryFormatter _formatter = new BinaryFormatter();

        public EventStore(IAppendOnlyStore<Guid> appendOnlyStore, IEventBus eventBus)
        {
            _appendOnlyStore = appendOnlyStore;
            _eventBus = eventBus;
        }

        public EventStream LoadEventStream(Guid id)
        {
            return LoadEventStream(id, 0, int.MaxValue);
        }

        public EventStream LoadEventStreamAfterVersion(Guid id, long afterVersion)
        {
            var records = _appendOnlyStore.ReadRecords(id, afterVersion, int.MaxValue).ToList();

            var stream = new EventStream();

            foreach (var tapeRecord in records)
            {
                stream.Events.AddRange(DeserializeEvent(tapeRecord.Data));
                stream.Version = tapeRecord.Version;
            }

            return stream;
        }

        public void AppendToStream<TType>(Guid id, long version, ICollection<IEvent> events,
            params IDomainEvent[] domainEvents)
        {
            //salvar os dados em uma lista interna
            var aggregateType = typeof(TType).Name;
            _appendOnlyStore.Append(id, aggregateType, version, events);
            _eventBus.Publish(domainEvents);
        }

        public bool Any(Guid id)
        {
            return _appendOnlyStore.Any(id);
        }

        public EventStream LoadEventStream(Guid id, int skipEvents, int maxCount)
        {
            var records = _appendOnlyStore.ReadRecords(id, skipEvents, maxCount).ToList();

            var stream = new EventStream();

            foreach (var tapeRecord in records)
            {
                stream.Events.AddRange(DeserializeEvent(tapeRecord.Data));
                stream.Version = tapeRecord.Version;
            }

            return stream;
        }

        private IEvent[] DeserializeEvent(byte[] data)
        {
            using (var mem = new MemoryStream(data))
            {
                return (IEvent[])_formatter.Deserialize(mem);
            }
        }
    }
}
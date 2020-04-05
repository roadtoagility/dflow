using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Core.Shared;
using Core.Shared.Base;
using Core.Shared.Base.Exceptions;
using Core.Shared.Interfaces;

namespace Program
{
    public class EventStore : IEventStore<Guid>
    {
        readonly BinaryFormatter _formatter = new BinaryFormatter();
        readonly IAppendOnlyStore<Guid> _appendOnlyStore;
        readonly IQueueService _queueService;
        
        public EventStore(IAppendOnlyStore<Guid> appendOnlyStore, IQueueService queueService)
        {
            _appendOnlyStore = appendOnlyStore;
            _queueService = queueService;
        }
        
        public EventStream LoadEventStream(Guid id) => LoadEventStream(id, 0, Int32.MaxValue);
        
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

        public EventStream LoadEventStreamAfterVersion(Guid id, long afterVersion)
        {
            var records = _appendOnlyStore.ReadRecords(id, afterVersion, Int32.MaxValue).ToList();

            var stream = new EventStream();

            foreach (var tapeRecord in records)
            {
                stream.Events.AddRange(DeserializeEvent(tapeRecord.Data));
                stream.Version = tapeRecord.Version;
            }

            return stream;
        }

        public void AppendToStream<TType>(Guid id, long version, ICollection<IEvent> events)
        {
            if (events.Count == 0)
                return;

            var data = SerializeEvent(events.ToArray());
            var aggregateType = typeof(TType).Name;

            var originalVersion = version - events.Count();

            if (_appendOnlyStore.Any(id))
            {
                var stream = new EventStream();
                foreach (var tapeRecord in _appendOnlyStore.ReadRecords(id, originalVersion, int.MaxValue))
                {
                    stream.Events.AddRange(DeserializeEvent(tapeRecord.Data));
                    stream.Version = tapeRecord.Version;
                }

                //Version = 0 significa que não existe stream anterior a versao informada no ReadRecords
                if (stream.Version > 0 && originalVersion != stream.Version)
                    throw new EventStoreConcurrencyException(stream.Events, stream.Version);
            }
            

            _appendOnlyStore.Append(id, aggregateType, data, version);
            _queueService.Publish(events.ToArray());
        }

        public bool Any(Guid id)
        {
            return _appendOnlyStore.Any(id);
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
    }
}
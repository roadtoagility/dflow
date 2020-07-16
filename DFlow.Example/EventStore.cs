using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using DFlow.Base;
using DFlow.Interfaces;

namespace DFlow.Example
{
    //TODO: talvez isso possa ser movido pra base
    public class EventStore : IEventStore<Guid>
    {
        
        readonly IAppendOnlyStore<Guid> _appendOnlyStore;
        readonly BinaryFormatter _formatter = new BinaryFormatter();
        
        public EventStore(IAppendOnlyStore<Guid> appendOnlyStore)
        {
            _appendOnlyStore = appendOnlyStore;
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
            var aggregateType = typeof(TType).Name;
            _appendOnlyStore.Append(id, aggregateType, version, events);
        }

        public bool Any(Guid id)
        {
            return _appendOnlyStore.Any(id);
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
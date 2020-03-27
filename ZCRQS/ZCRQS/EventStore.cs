using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Core.Shared;

namespace Program
{
    public class EventStore : IEventStore
    {
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
        
        public EventStream LoadEventStream(Guid id) => LoadEventStream(id, 0, Int32.MaxValue);
        
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
    }
}
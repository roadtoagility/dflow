using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Core.Shared
{
    public interface IEvent
    {
        string GetEventName();
        Guid GetRoot();
    }
    
    public interface IEventStore
    {
        EventStream LoadEventStream(Guid id);
        EventStream LoadEventStream(Guid id, int skipEvents, int maxCOunt);

        void AppendToStream(Guid id, long expectedVersion, ICollection<IEvent> events);
    }
    
    public class EventStore : IEventStore
    {
        readonly IAppendOnlyStore _appendOnlyStore;
        readonly BinaryFormatter _formatter = new BinaryFormatter();
        public EventStream LoadEventStream(Guid id)
        {
            throw new NotImplementedException();
        }

        public EventStream LoadEventStream(Guid id, int skip, int take)
        {
            var name = IdentityToString(id);

            var records = _appendOnlyStore.ReadRecords(name, skip, take).ToList();

            var stream = new EventStream();

            foreach (var tapeRecord in records)
            {

                stream.Events.AddRange(DeserializeEvent(tapeRecord.Data));

                stream.Version = tapeRecord.Version;

            }
            return stream;
        }

        public void AppendToStream(Guid id, long originalVersion, ICollection<IEvent> events)

        {

            if (events.Count == 0)
                return;

            var name = IdentityToString(id);

            var data = SerializeEvent(events.ToArray());

            try
            {
                _appendOnlyStore.Append(name, data, originalVersion);
            }

            catch(Exception e)//catch(AppendOnlyStoreConcurrencyException e)
            {
                // load server events

                var server = LoadEventStream(id, 0, int.MaxValue);
                // throw a real problem
                throw e;
                //throw OptimisticConcurrencyException.Create(server.Version, e.ExpectedVersion, id, server.Events);
            }
        }



        string IdentityToString(Guid id)
        {
            // in this project all identities produce proper name

            return id.ToString();
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

    public class EventStream
    {
        public int Version;
        public List<IEvent> Events;
    }
    
    
    public interface IAppendOnlyStore : IDisposable

    {

        void Append(string name, byte[] data, long expectedVersion = -1);

        IEnumerable<DataWithVersion> ReadRecords(string name, long afterVersion, int maxCount);

        IEnumerable<DataWithName> ReadRecords(long afterVersion, int maxCount);



        void Close();

    }

    public class AppendOnlyStore : IAppendOnlyStore
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Append(string name, byte[] data, long expectedVersion = -1)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DataWithVersion> ReadRecords(string name, long afterVersion, int maxCount)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DataWithName> ReadRecords(long afterVersion, int maxCount)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
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
}
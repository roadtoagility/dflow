using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using DFlow.Base.Exceptions;
using DFlow.Interfaces;

namespace DFlow.Base
{
    public abstract class AppendOnlyBase
    {
        readonly BinaryFormatter _formatter = new BinaryFormatter();

        public void Append(Guid id, string aggregateType, long version, ICollection<IEvent> events)
        {
            if (events.Count == 0)
            {
                return;
            }

            var data = SerializeEvent(events.ToArray());

            var originalVersion = version - events.Count();

            if (Any(id))
            {
                var stream = new EventStream();
                foreach (var tapeRecord in ReadRecords(id, originalVersion, int.MaxValue))
                {
                    stream.Events.AddRange(DeserializeEvent(tapeRecord.Data));
                    stream.Version = tapeRecord.Version;
                }

                //Version = 0 significa que não existe stream anterior a versao informada no ReadRecords
                if (stream.Version > 0 && originalVersion != stream.Version)
                {
                    throw new EventStoreConcurrencyException(stream.Events, stream.Version);
                }
            }
            
            Save(id, aggregateType, version, data);
        }

        protected abstract void Save(Guid id, string aggregateType, long version, byte[] data);
        public abstract bool Any(Guid aggregateId);
        public abstract IEnumerable<DataWithVersion> ReadRecords(Guid aggregateId, long afterVersion, int maxCount);
        
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using DFlow.Domain.BusinessObjects;
using DFlow.Persistence.EventStore.Model;
using Microsoft.EntityFrameworkCore;

namespace DFlow.Persistence.EventStore.Repositories
{
    public class EventStoreRepository<Guid> : IEventStoreRepository<Guid>
    {
        
        private readonly AggregateDbContext _dbContext;
        private readonly BinaryFormatter _formatter = new BinaryFormatter();
        
        public EventStoreRepository(AggregateDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        // public EventStream<TAgregateId> LoadEventStream(Guid id) => LoadEventStream(id, 0, Int32.MaxValue);
        
        public EventStream<Guid> LoadEventStream(Guid aggregateId, int skipEvents, int maxCount)
        {
            var records = null;//_appendOnlyStore.ReadRecords(id, skipEvents, maxCount).ToList();

            var stream = EventStream<Guid>.From(records);

            return stream;
        }

        // public EventStream LoadEventStreamAfterVersion(Guid id, long afterVersion)
        // {
        //     var records = _appendOnlyStore.ReadRecords(id, afterVersion, Int32.MaxValue).ToList();
        //
        //     var stream = new EventStream();
        //
        //     foreach (var tapeRecord in records)
        //     {
        //         stream.Events.AddRange(DeserializeEvent(tapeRecord.Data));
        //         stream.Version = tapeRecord.Version;
        //     }
        //
        //     return stream;
        // }

        public void Append<TType>(Guid aggregateId, EventStream<Guid> aggregateStream)
        {
            var records = LoadEventStream(aggregateStream.AggregationId);
            var events = new List<EventData<Guid>>();
            
            // foreach (var tapeRecord in records)
            // {
            stream.Events.ToList().ForEach(ev =>
            {
                events.Add(new EventData<Guid>(aggregateId,stream.Name, 
                    BitConverter.GetBytes(stream.Version.Value),
                    DeserializeEvent(ev.Data));
            });
            // }
 
                
            //salvar os dados em uma lista interna
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
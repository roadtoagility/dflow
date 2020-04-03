using System;
using System.Collections.Generic;
using System.Linq;
using Core.Shared;
using Core.Shared.Base;
using Core.Shared.Interfaces;

namespace Program
{
    public class MemoryAppendOnlyStore : IAppendOnlyStore<Guid>
    {
        //Aqui seria a infra que insere no banco
        private ICollection<EventDTO<Guid>> _eventsStorage = new List<EventDTO<Guid>>();
        
        public void Dispose()
        {
            _eventsStorage = new List<EventDTO<Guid>>();
        }

        public void Append(Guid aggregateId, string aggregateType, byte[] data, long expectedVersion = -1)
        {
            //(Guid aggregateId, string aggregateType, Guid eventId, int version, byte[] data)
            _eventsStorage.Add(new EventDTO<System.Guid>(aggregateId, aggregateType, expectedVersion, data));
        }

        public IEnumerable<DataWithVersion> ReadRecords(string aggregateType, long afterVersion, int maxCount)
        {
            return _eventsStorage
                .Where(x => x.AggregateType.Equals(aggregateType) && x.Version > afterVersion)
                .Take(maxCount)
                .Select(x => new DataWithVersion(x.Version, x.Data));
        }

        public IEnumerable<DataWithVersion> ReadRecords(Guid aggregateId, long afterVersion, int maxCount)
        {
            return _eventsStorage
                .Where(x => x.AggregateId == aggregateId && x.Version > afterVersion)
                .Take(maxCount)
                .Select(x => new DataWithVersion(x.Version, x.Data));
        }
        
        public IEnumerable<DataWithVersion> ReadRecords<T>(long afterVersion, int maxCount)
        {
            var aggregateType = typeof(T).Name;
            return _eventsStorage
                .Where(x => x.AggregateType.Equals(aggregateType) && x.Version > afterVersion)
                .Take(maxCount)
                .Select(x => new DataWithVersion(x.Version, x.Data));
        }

        public IEnumerable<DataWithName> ReadRecords(long afterVersion, int maxCount)
        {
            return _eventsStorage
                .Where(x => x.Version > afterVersion)
                .Take(maxCount)
                .Select(x => new DataWithName(x.AggregateType, x.Data));
        }

        public void Close()
        {
            _eventsStorage = null;
        }

        //DTO para poder inserir em qualquer modelo de banco
        private class EventDTO<TKey>
        {
            public EventDTO(TKey aggregateId, string aggregateType, long version, byte[] data)
            {
                AggregateId = aggregateId;
                AggregateType = aggregateType;
                Version = version;
                Data = data;
            }

            public TKey AggregateId { get; set; }
            
            public string AggregateType { get; set; }
            public long Version { get; set; }
            public byte[] Data { get; set; }
        }
    }
}
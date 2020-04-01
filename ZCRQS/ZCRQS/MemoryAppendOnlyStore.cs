using System;
using System.Collections.Generic;
using System.Linq;
using Core.Shared;

namespace Program
{
    public class MemoryAppendOnlyStore : IAppendOnlyStore
    {
        //Aqui seria a infra que insere no banco
        private ICollection<EventDTO> _eventsStorage = new List<EventDTO>();
        
        public void Dispose()
        {
            _eventsStorage = new List<EventDTO>();
        }

 

        public void Append(Guid aggregateId, string aggregateType, byte[] data, int expectedVersion = -1)
        {
            //(Guid aggregateId, string aggregateType, Guid eventId, int version, byte[] data)
            _eventsStorage.Add(new EventDTO(aggregateId, aggregateType, expectedVersion, data));
        }

        public IEnumerable<DataWithVersion> ReadRecords(string aggregateType, int afterVersion, int maxCount)
        {
            return _eventsStorage
                .Where(x => x.AggregateType.Equals(aggregateType) && x.Version > afterVersion)
                .Take(maxCount)
                .Select(x => new DataWithVersion(x.Version, x.Data));
        }
        
        public IEnumerable<DataWithVersion> ReadRecords(Guid aggregateId, int afterVersion, int maxCount)
        {
            return _eventsStorage
                .Where(x => x.AggregateId == aggregateId && x.Version > afterVersion)
                .Take(maxCount)
                .Select(x => new DataWithVersion(x.Version, x.Data));
        }
        
        public IEnumerable<DataWithVersion> ReadRecords<T>(int afterVersion, int maxCount)
        {
            var aggregateType = typeof(T).Name;
            return _eventsStorage
                .Where(x => x.AggregateType.Equals(aggregateType) && x.Version > afterVersion)
                .Take(maxCount)
                .Select(x => new DataWithVersion(x.Version, x.Data));
        }

        public IEnumerable<DataWithName> ReadRecords(int afterVersion, int maxCount)
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
        private class EventDTO
        {
            public EventDTO(Guid aggregateId, string aggregateType, int version, byte[] data)
            {
                AggregateId = aggregateId;
                AggregateType = aggregateType;
                Version = version;
                Data = data;
            }

            public Guid AggregateId { get; set; }
            
            public string AggregateType { get; set; }
            public int Version { get; set; }
            public byte[] Data { get; set; }
        }
    }
}
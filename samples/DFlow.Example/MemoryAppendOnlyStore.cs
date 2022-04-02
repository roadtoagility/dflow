using System;
using System.Collections.Generic;
using System.Linq;
using DFlow.Base;
using DFlow.Interfaces;

namespace DFlow.Example
{
    public class MemoryAppendOnlyStore : AppendOnlyBase, IAppendOnlyStore<Guid>
    {
        private ICollection<EventDTO<Guid>> _eventsStorage = new List<EventDTO<Guid>>();


        public MemoryAppendOnlyStore(IEventBus eventBus)
        {
        }

        public void Dispose()
        {
            _eventsStorage = new List<EventDTO<Guid>>();
        }

        public IEnumerable<DataWithVersion> ReadRecords(string aggregateType, long afterVersion, int maxCount)
        {
            return _eventsStorage
                .Where(x => x.AggregateType.Equals(aggregateType) && x.Version > afterVersion)
                .Take(maxCount)
                .Select(x => new DataWithVersion(x.Version, x.Data));
        }

        public override IEnumerable<DataWithVersion> ReadRecords(Guid aggregateId, long afterVersion, int maxCount)
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

        public override bool Any(Guid aggregateId)
        {
            return _eventsStorage.Any(x => x.AggregateId == aggregateId);
        }

        public void Close()
        {
        }

        protected override void Save(Guid id, string aggregateType, long version, byte[] data)
        {
            _eventsStorage.Add(new EventDTO<Guid>(id, aggregateType, version, data));
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

            public TKey AggregateId { get; }

            public string AggregateType { get; }
            public long Version { get; }
            public byte[] Data { get; }
        }
    }
}
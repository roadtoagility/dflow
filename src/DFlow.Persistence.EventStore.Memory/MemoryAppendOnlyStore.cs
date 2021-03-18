using System;
using System.Collections.Generic;
using System.Linq;
using DFlow.Persistence.EventStore.Model;
using DFlow.Persistence.EventStore.Repositories;

namespace DFlow.Persistence.EventStore.InMemoryStore
{
    public class MemoryAppendOnlyStore : AppendOnlyBase<Guid>
    {
        private ICollection<EventData<Guid>> _eventsStorage = new List<EventData<Guid>>();

        public void Dispose()
        {
            _eventsStorage = new List<EventData<Guid>>();
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
            _eventsStorage.Add(new EventData<System.Guid>(id, aggregateType, version, data));
        }
    }
}
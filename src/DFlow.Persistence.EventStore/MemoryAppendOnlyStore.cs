using System;
using System.Collections.Generic;
using System.Linq;
using DFlow.Persistence.EventStore.Model;

namespace DFlow.Persistence.EventStore
{
    public class MemoryAppendOnlyStore //: AppendOnlyBase, IAppendOnlyStore<Guid>
    {
        private readonly ICollection<EventData<Guid>> _eventsStorage = new List<EventData<Guid>>();

        public IEnumerable<DataWithVersion> ReadRecords(string aggregateType, long afterVersion, int maxCount)
        {
            return _eventsStorage
                .Where(x => x.AggregateType.Equals(aggregateType) && x.Version > afterVersion)
                .Take(maxCount)
                .Select(x => new DataWithVersion(x.Version, x.Data));
        }

        public  IEnumerable<DataWithVersion> ReadRecords(Guid aggregateId, long afterVersion, int maxCount)
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

        public  bool Any(Guid aggregateId)
        {
            return _eventsStorage.Any(x => x.AggregateId == aggregateId);
        }

        protected  void Save(Guid id, string aggregateType, long version, byte[] data)
        {
            _eventsStorage.Add(new EventData<System.Guid>(id, aggregateType, version, data));
        }
    }
}
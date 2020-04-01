using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Core.Shared;

namespace Program
{
    public class SnapshotRepository : ISnapshotRepository
    {
        internal class SnapshotAggregate
        {
            public Guid Id { get; set; }
            public long Version { get; set; }
            public byte[] Data { get; set; }
        }
        
        private ICollection<SnapshotAggregate> _snapshotStorage = new List<SnapshotAggregate>();
        readonly BinaryFormatter _formatter = new BinaryFormatter();
        
        public SnapshotRepository()
        {
            
        }
        
        public bool TryGetSnapshotById<TAggregate>(Guid id, out TAggregate snapshot, out long version)
            where TAggregate : AggregateRoot
        {
            var aggregate = _snapshotStorage
                .Where(x => x.Id == id)
                .OrderByDescending(x => x.Version)
                .FirstOrDefault();

            if (aggregate == null)
            {
                version = 0;
                snapshot = default(TAggregate);
                return false;
            }

            version = aggregate.Version;
            var agg = Deserialize(aggregate.Data);
            snapshot = (TAggregate)agg;

            return true;
        }

        public void SaveSnapshot<TAggregate>(Guid id, TAggregate snapshot, int version)
            where TAggregate : AggregateRoot
        {
            var data = Serialize(snapshot);
            _snapshotStorage.Add(new SnapshotAggregate(){Data = data, Id = id, Version = version });
        }
        
        byte[] Serialize(AggregateRoot e)
        {
            using (var mem = new MemoryStream())
            {
                _formatter.Serialize(mem, e);
                return mem.ToArray();
            }
        }
        
        AggregateRoot Deserialize(byte[] data)
        {
            using (var mem = new MemoryStream(data))
            {
                return (AggregateRoot)_formatter.Deserialize(mem);
            }
        }
    }
}
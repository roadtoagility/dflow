using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Core.Shared;
using Core.Shared.Base;
using Core.Shared.Base.Aggregate;
using Core.Shared.Interfaces;

namespace Program
{
    public class SnapshotRepository : ISnapshotRepository<Guid>
    {
        internal class SnapshotAggregate<TKey>
        {
            public TKey Id { get; set; }
            public long Version { get; set; }
            public byte[] Data { get; set; }
        }
        
        private ICollection<SnapshotAggregate<Guid>> _snapshotStorage = new List<SnapshotAggregate<Guid>>();
        readonly BinaryFormatter _formatter = new BinaryFormatter();
        
        public SnapshotRepository()
        {
            
        }
        
        public bool TryGetSnapshotById<TAggregate>(Guid id, out TAggregate snapshot, out long version) where TAggregate : AggregateRoot<Guid>
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

        public void SaveSnapshot<TAggregate>(Guid id, TAggregate snapshot, long version) where TAggregate : AggregateRoot<Guid>
        {
            /*o snapshot é a materialização da agregação, ou seja, é um arquivo desnormalizado com todas as informações
             da agregação em um determinado ponto do tempo, só que em vez de criar um DTO pra isso, resolvi apenas remover as changes e 
             serializar a própria agregação
             */
            snapshot.Changes.Clear();
            var data = Serialize(snapshot);
            _snapshotStorage.Add(new SnapshotAggregate<Guid>(){Data = data, Id = id, Version = version });
        }
        
        byte[] Serialize(AggregateRoot<Guid> e)
        {
            using (var mem = new MemoryStream())
            {
                _formatter.Serialize(mem, e);
                return mem.ToArray();
            }
        }
        
        AggregateRoot<Guid> Deserialize(byte[] data)
        {
            using (var mem = new MemoryStream(data))
            {
                return (AggregateRoot<Guid>)_formatter.Deserialize(mem);
            }
        }
    }
}
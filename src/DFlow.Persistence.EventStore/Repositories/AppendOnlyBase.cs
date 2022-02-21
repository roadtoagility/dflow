// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using DFlow.Domain.Events;
using DFlow.Persistence.EventStore.Model;

namespace DFlow.Persistence.EventStore.Repositories
{
    public abstract class AppendOnlyBase<TKey> //:IAppendOnlyBase<TKey>
    {
        readonly BinaryFormatter _formatter = new BinaryFormatter();

        public void Append(Guid id, string aggregateType, long version, ICollection<IDomainEvent> events)
        {
            if (events.Count == 0)
            {
                return;
            }

            var data = SerializeEvent(events.ToArray());

            var originalVersion = version - events.Count();

            if (Any(id))
            {
                // var stream = new EventStream();
                // foreach (var tapeRecord in ReadRecords(id, originalVersion, int.MaxValue))
                // {
                //     stream.Events.AddRange(DeserializeEvent(tapeRecord.Data));
                //     stream.Version = tapeRecord.Version;
                // }

                // //Version = 0 significa que não existe stream anterior a versao informada no ReadRecords
                // if (stream.Version > 0 && originalVersion != stream.Version)
                // {
                //     throw new EventStoreConcurrencyException(stream.Events, stream.Version);
                // }
            }
            
            Save(id, aggregateType, version, data);
        }

        protected abstract void Save(Guid id, string aggregateType, long version, byte[] data);
        public abstract bool Any(Guid aggregateId);
        public abstract IEnumerable<DataWithVersion> ReadRecords(Guid aggregateId, long afterVersion, int maxCount);
        
        byte[] SerializeEvent(IDomainEvent[] e)
        {
            using (var mem = new MemoryStream())
            {
                _formatter.Serialize(mem, e);
                return mem.ToArray();
            }
        }
        
        IDomainEvent[] DeserializeEvent(byte[] data)
        {
            using (var mem = new MemoryStream(data))
            {
                return (IDomainEvent[])_formatter.Deserialize(mem);
            }
        }
    }
}
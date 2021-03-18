// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System;
using System.Collections.Generic;
using DFlow.Domain.Events;
using DFlow.Persistence.EventStore.Model;

namespace DFlow.Persistence.EventStore.Repositories
{
    public interface IAppendOnlyBase<TKey>: IDisposable
    {
        void Append(Guid id, string aggregateType, long version, ICollection<IDomainEvent> events);

        IEnumerable<DataWithVersion> ReadRecords(string name, long afterVersion, int maxCount);
        IEnumerable<DataWithVersion> ReadRecords(TKey aggregateId, long afterVersion, int maxCount);
        IEnumerable<DataWithVersion> ReadRecords<T>(long afterVersion, int maxCount);

        IEnumerable<DataWithName> ReadRecords(long afterVersion, int maxCount);
        
        bool Any(TKey aggregateId);

        void Close();

    }
}
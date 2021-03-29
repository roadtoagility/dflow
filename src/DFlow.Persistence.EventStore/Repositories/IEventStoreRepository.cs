// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Collections.Generic;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Events;

namespace DFlow.Persistence.EventStore.Repositories
{
    public interface IEventStoreRepository<TKey>
    {
        EventStream<TKey> LoadEventStream(TKey aggregateId, int version);

        void Append(TKey id, EventStream<TKey> aggregateStream);
    }
}
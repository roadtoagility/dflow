// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


namespace DFlow.Persistence.EventStore.Snapshoting
{
    public interface ISnapshotRepository<TKey>
    {
        // bool TryGetSnapshotById<TAggregate>(TKey id, out TAggregate snapshot, out long version)
        //     where TAggregate : AggregateRoot<TKey>;
        //
        // void SaveSnapshot<TAggregate>(TKey id, TAggregate snapshot, long version) where TAggregate : AggregateRoot<TKey>;
    }
}
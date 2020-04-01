using System;

namespace Core.Shared
{
    public interface ISnapshotRepository
    {
        bool TryGetSnapshotById<TAggregate>(Guid id, out TAggregate snapshot, out long version)
            where TAggregate : AggregateRoot;

        void SaveSnapshot<TAggregate>(Guid id, TAggregate snapshot, int version) where TAggregate : AggregateRoot;
    }
}
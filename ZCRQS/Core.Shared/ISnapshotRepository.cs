using System;

namespace Core.Shared
{
    public interface ISnapshotRepository
    {
        bool TryGetSnapshotById<TAggregate>(Guid id, out TAggregate snapshot, out int version);
        void SaveSnapshot<TAggregate>(Guid id, TAggregate snapshot, int version);
    }
}
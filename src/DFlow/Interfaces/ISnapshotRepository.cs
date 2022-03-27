using DFlow.Base.Aggregate;

namespace DFlow.Interfaces
{
    public interface ISnapshotRepository<TKey>
    {
        bool TryGetSnapshotById<TAggregate>(TKey id, out TAggregate snapshot, out long version)
            where TAggregate : AggregateRoot<TKey>;

        void SaveSnapshot<TAggregate>(TKey id, TAggregate snapshot, long version)
            where TAggregate : AggregateRoot<TKey>;
    }
}
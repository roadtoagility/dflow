using System;
using DFlow.Interfaces;

namespace DFlow.Base.Aggregate
{
    [Serializable]
    public sealed class AggregateCreated<TKey> : IEvent
    {
        public AggregateCreated(TKey id)
        {
            Id = id;
        }

        public TKey Id { get; private set; }
    }
}
using System;
using Core.Shared.Interfaces;

namespace Core.Shared.Base.Aggregate
{
    [Serializable]
    public sealed class AggregateCreated<TKey> : IEvent
    {
        public TKey Id { get; private set; }
        
        public AggregateCreated(TKey id)
        {
            Id = id;
        }
    }
}
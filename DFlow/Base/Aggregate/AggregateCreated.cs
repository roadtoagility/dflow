using System;
using DFlow.Interfaces;

namespace DFlow.Base.Aggregate
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
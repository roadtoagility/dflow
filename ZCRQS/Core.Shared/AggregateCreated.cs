using System;

namespace Core.Shared
{
    [Serializable]
    public sealed class AggregateCreated : IEvent
    {
        public Guid Id { get; private set; }
        
        public AggregateCreated(Guid id)
        {
            Id = id;
        }
    }
}
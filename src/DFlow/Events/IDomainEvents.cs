using System.Collections.Generic;

namespace DFlow.Events
{
    public interface IDomainEvents
    {
        IReadOnlyList<DomainEvent> GetEvents();
    }
}
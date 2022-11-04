using System.Collections.Generic;
using DFlow.Events;

namespace DFlow.BusinessObjects
{
    public interface IDomainEvents
    {
        IReadOnlyList<DomainEvent> GetEvents();
    }
}    
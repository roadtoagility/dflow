using System.Text.Json;
using NodaTime;

namespace Ecommerce.Persistence.State;

public sealed record AggregateState(Guid Id
    , Guid AggregateId
    , string AggregationType
    , string EventType
    , Instant EventDatetime
    , JsonDocument EventData):IDisposable
{
    public void Dispose()
    {
        EventData.Dispose();
    }
}
using System;
using System.Text.Json;
using NodaTime;

namespace DFlow.Persistence.Outbox;

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
using System;
using System.Text.Json;

namespace DFlow.Testing.Supporting.Persistence;

public sealed record ChangeSet(Guid Id
    , Guid AggregateId
    , string AggregationType
    , string EventType
    , DateTimeOffset EventDatetime
    , JsonDocument EventData) : IDisposable
{
    public void Dispose()
    {
        EventData.Dispose();
    }
}
// // Copyright (C) 2023  Road to Agility

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using DFlow.Persistence.Outbox;
using DFlow.Testing.Supporting.DomainObjects;

namespace DFlow.Testing.Supporting.Persistence;

public sealed class ExportPrimaryEntityEvents : IEventsExporting<ChangeSet, PrimaryEntity>
{
    public IReadOnlyList<ChangeSet> ToOutBox(PrimaryEntity fromEntity)
    {
        var aggId = Guid.NewGuid(); //aggregate id for all events of changeset
        return fromEntity.GetEvents().Select(evt => new ChangeSet(
            aggId,
            fromEntity.Identity.Value,
            fromEntity.GetType().Name,
            evt.GetType().Name,
            DateTimeOffset.Now, 
            JsonSerializer.SerializeToDocument(evt, evt.GetType())
        )).ToImmutableList();
    }
}
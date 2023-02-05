// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections.Immutable;
using System.Text.Json;
using Ecommerce.Domain;
using Ecommerce.Persistence.State;
using NodaTime;

namespace Ecommerce.Persistence.ExtensionMethods;

public static class EventsOutboxExporting
{
    public static IReadOnlyList<AggregateState> ToOutBox(this Product entityAggregateRoot)
    {
        return entityAggregateRoot.GetEvents().Select(e => new AggregateState(
            Guid.NewGuid(),
            entityAggregateRoot.Identity.Value,
            entityAggregateRoot.GetType().Name,
            e.GetType().Name,
            Instant.FromDateTimeOffset(DateTimeOffset.Now), 
            JsonSerializer.SerializeToDocument(e, e.GetType())
        )).ToImmutableList();
    }
}
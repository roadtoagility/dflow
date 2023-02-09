// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using DFlow.Events;

namespace DFlow.Testing.Supporting.DomainObjects.Events;

// https://medium.com/the-sixt-india-blog/primitive-obsession-code-smell-that-hurt-people-the-most-5cbdd70496e9

public class SecondaryEntityUpdatedEvent : DomainEvent
{
    public SecondaryEntityUpdatedEvent(PrimaryEntityId id, SecondaryEntity secondary, DateTimeOffset when)
        : base(when)
    {
        Id = id.Value;
        SecondaryEntityId = secondary.Identity.Value;
        SimpleValue = secondary.SimpleValue.Value;
    }

    public Guid Id { get; }
    public string SimpleValue { get; }
    public Guid SecondaryEntityId { get; }

    public static SecondaryEntityUpdatedEvent For(PrimaryEntity entity)
    {
        return new(
            entity.Identity,
            entity.Secondary,
            DateTimeOffset.UtcNow);
    }
}
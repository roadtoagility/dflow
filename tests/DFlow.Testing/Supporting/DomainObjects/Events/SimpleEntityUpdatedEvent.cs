// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using DFlow.Events;
using Ecommerce.Domain;

namespace DFlow.Testing.Supporting.DomainObjects.Events;

// https://medium.com/the-sixt-india-blog/primitive-obsession-code-smell-that-hurt-people-the-most-5cbdd70496e9

public class SimpleEntityUpdatedEvent : DomainEvent
{
    public SimpleEntityUpdatedEvent(SimpleEntityId id, SimpleValueObject simpleValue, DateTimeOffset when)
        : base(when)
    {
        Id = id.Value;
        SimpleValue = simpleValue.Value;
    }

    public Guid Id { get; }
    public string SimpleValue { get; }

    public static SimpleEntityUpdatedEvent For(SimpleEntity simpleEntity)
    {
        return new (
            simpleEntity.Identity,
            simpleEntity.SimpleValue,
            DateTimeOffset.UtcNow);
    }
}
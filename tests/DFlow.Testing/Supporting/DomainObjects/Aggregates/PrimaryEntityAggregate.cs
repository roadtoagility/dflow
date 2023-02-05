// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using DFlow.Aggregates;
using DFlow.Testing.Supporting.DomainObjects.Events;

namespace DFlow.Testing.Supporting.DomainObjects.Aggregates;

public sealed class PrimaryEntityAggregate : AggregateBase<PrimaryEntity, PrimaryEntityId>
{
    private PrimaryEntityAggregate(PrimaryEntity primaryEntity)
        : base(primaryEntity)
    {
        if (primaryEntity.IsValid)
        {
            Apply(primaryEntity);

            if (primaryEntity.IsNew())
            {
                Raise(PrimaryEntityCreatedEvent.For(primaryEntity));
            }
        }
        else
        {
            AppendValidationResult(primaryEntity.Failures);
        }
    }

    public void Update(SecondaryEntity secondary)
    {
        var primary = PrimaryEntity.Combine(Root, secondary);

        if (primary.IsValid)
        {
            Apply(primary);
            Raise(SecondaryEntityUpdatedEvent.For(primary));
        }

        AppendValidationResult(primary.Failures);
    }

    public static PrimaryEntityAggregate CreateFrom(SecondaryEntity secondary, SimpleValueObject simpleValue)
    {
        return new PrimaryEntityAggregate(PrimaryEntity.NewEntity(secondary, simpleValue));
    }
}
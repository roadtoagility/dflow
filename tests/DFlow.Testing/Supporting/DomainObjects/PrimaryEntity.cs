// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using DFlow.BusinessObjects;
using DFlow.Testing.Supporting.DomainObjects.Events;

namespace DFlow.Testing.Supporting.DomainObjects;

public class PrimaryEntity : EntityBase<PrimaryEntityId>
{
    public PrimaryEntity(PrimaryEntityId identity, SecondaryEntity secondaryEntity, SimpleValueObject simpleValue,
        VersionId version)
        : base(identity, version)
    {
        SimpleValue = simpleValue;
        Secondary = secondaryEntity;

        AppendValidationResult(identity.ValidationStatus.Failures);
        AppendValidationResult(simpleValue.ValidationStatus.Failures);
        AppendValidationResult(secondaryEntity.Failures);
    }

    public SimpleValueObject SimpleValue { get; private set; }
    public SecondaryEntity Secondary { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Identity;
        yield return SimpleValue;
        yield return Secondary;
    }

    public static PrimaryEntity From(PrimaryEntityId id, SecondaryEntity secondary, SimpleValueObject simpleObject,
        VersionId version)
    {
        return new PrimaryEntity(id, secondary, simpleObject, version);
    }

    public static PrimaryEntity NewEntity(SecondaryEntity secondary, SimpleValueObject simpleObject)
    {
        var entity = From(PrimaryEntityId.NewId(), secondary, simpleObject, VersionId.New());
        entity.RaisedEvent(PrimaryEntityCreatedEvent.For(entity));
        return entity;
    }

    public static PrimaryEntity Combine(PrimaryEntity primary, SecondaryEntity secondary)
    {
        return From(primary.Identity, secondary, primary.SimpleValue, VersionId.Next(primary.Version));
    }

    public static PrimaryEntity Empty()
    {
        return From(PrimaryEntityId.Empty, SecondaryEntity.Empty(), SimpleValueObject.Empty(), VersionId.Empty());
    }

    public void Update(SimpleValueObject simpleValue)
    {
        if (!simpleValue.ValidationStatus.IsValid)
        {
            AppendValidationResult(simpleValue.ValidationStatus.Failures);
        }

        SimpleValue = simpleValue;
        RaisedEvent(SimpleValueUpdatedEvent.For(this));
    }

    public void UpdateSecondary(SecondaryEntity secondary)
    {
        if (!secondary.IsValid)
        {
            AppendValidationResult(secondary.Failures);
        }

        Secondary = secondary;
        RaisedEvent(SecondaryEntityUpdatedEvent.For(this));
    }
}
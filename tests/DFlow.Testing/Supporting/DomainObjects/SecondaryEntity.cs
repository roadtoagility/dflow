// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using DFlow.BusinessObjects;
using DFlow.Testing.Supporting.DomainObjects.Events;
using DFlow.Validation;
using Ecommerce.Domain;

namespace DFlow.Testing.Supporting.DomainObjects;

public sealed class SecondaryEntity : EntityBase<SecondaryEntityId>
{
    public SecondaryEntity(SecondaryEntityId identity, SimpleValueObject simpleValue, VersionId version)
        : base(identity, version)
    {
        

        AppendValidationResult(identity.ValidationStatus.Failures);
        AppendValidationResult(simpleValue.ValidationStatus.Failures);
    }

    public SimpleValueObject SimpleValue { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Identity;
        yield return SimpleValue;
    }

    public static SecondaryEntity From(SecondaryEntityId id, SimpleValueObject simpleObject, VersionId version)
    {
        return new SecondaryEntity(id, simpleObject, version);
    }

    public static SecondaryEntity NewEntity(SimpleValueObject simpleObject)
    {
        return From(SecondaryEntityId.NewId(), simpleObject, VersionId.New());
    }

    public static SecondaryEntity Empty()
    {
        return From(SecondaryEntityId.Empty, SimpleValueObject.Empty(), VersionId.Empty());
    }
}
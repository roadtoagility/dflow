// Copyright (C) 2023  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.Testing.Supporting.DataProviders;
using DFlow.Testing.Supporting.DomainObjects;
using DFlow.Testing.Supporting.DomainObjects.Events;
using Xunit;

namespace DFlow.Testing.Domain;

public class PrimaryEntityTests
{
    [Theory]
    [ClassData(typeof(PrimaryEntityValid))]
    public void PrimaryEntity_Create_Valid(SecondaryEntity secondary, SimpleValueObject name, int expected)
    {
        var entity = PrimaryEntity.NewEntity(secondary,name);
        Assert.Equal(expected, entity.GetEvents().Count);
    }
    
    [Theory]
    [ClassData(typeof(SecondaryEntityUpdate))]
    public void SecondaryEntity_Updated(PrimaryEntity primary, SecondaryEntity toUpdate, SecondaryEntity expected)
    {
        primary.UpdateSecondary(toUpdate);
        var raisedEvent = primary.GetEvents()[0] as SecondaryEntityUpdatedEvent;
        Assert.Equal(expected.Identity.Value, raisedEvent.SecondaryEntityId);
    }
}
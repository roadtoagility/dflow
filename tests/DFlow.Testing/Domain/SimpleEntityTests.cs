// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.BusinessObjects;
using DFlow.Testing.Supporting.DataProviders;
using DFlow.Testing.Supporting.DomainObjects;
using Xunit;

namespace DFlow.Testing.Domain;

public class SimpleEntityTests
{
    [Theory]
    [ClassData(typeof(SimpleEntityValid))]
    public void SimpleEntity_Create_Valid(
        SimpleEntityId simpleEntityId,
        SimpleValueObject name,
        VersionId versionId, SimpleEntity expected)
    {
        var entity = SimpleEntity.From(simpleEntityId, name, versionId);
        Assert.Equal(expected, entity);
    }

    [Theory]
    [ClassData(typeof(SimpleEntityInvalid))]
    public void SimpleEntity_Create_InValid(SimpleEntity entity, bool expected)
    {
        Assert.Equal(expected, entity.IsValid);
    }
}
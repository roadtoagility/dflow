// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.BusinessObjects;
using DFlow.Testing.Supporting.DataProviders;
using DFlow.Testing.Supporting.DomainObjects;
using Ecommerce.Domain;
using Xunit;

namespace Ecommerce.Tests.Domain;

public class SimpleEntityTests
{
    [Theory]
    [ClassData(typeof(SimpleEntityValid))]
    public void CreateValidProduct(
        SimpleEntityId simpleEntityId,
        SimpleValueObject name,
        VersionId versionId, SimpleEntity expected)
    {
        var entity = SimpleEntity.From(simpleEntityId, name, versionId);
        Assert.Equal(expected, entity);
    }
}
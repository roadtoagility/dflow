// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.BusinessObjects;
using DFlow.Testing.Supporting.DataProviders;
using DFlow.Testing.Supporting.DomainObjects;
using DFlow.Testing.Supporting.DomainObjects.Aggregates;
using Xunit;

namespace DFlow.Testing.Domain;

public class AggregateEntityTests
{
    [Theory]
    [ClassData(typeof(PrimaryEntityForAggregate))]
    public void CreatePrimaryAggregate(SecondaryEntity inputSecondary, SimpleValueObject inputValue,
        PrimaryEntity expected)
    {
        var agg = PrimaryEntityAggregate.CreateFrom(inputSecondary, inputValue);
        Assert.Equal(expected.Secondary, agg.GetChange().Secondary);
        Assert.Equal(expected.SimpleValue, agg.GetChange().SimpleValue);
    }
}
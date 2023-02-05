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

public class BusinessObjectTests
{
    [Theory]
    [InlineData(1, true)]
    public void CreateNewVersion(int input, bool expectedTrue)
    {
        var versionId = VersionId.From(input);
        Assert.Equal(expectedTrue,  versionId.IsNew);
    }
    [Theory]
    [ClassData(typeof(SimpleValueObjectValid))]
    public void CreateValidVO(string input, SimpleValueObject expectedValid)
    {
        var validVO = SimpleValueObject.From(input);
        Assert.Equal(expectedValid, validVO);
    }

    [Theory]
    [InlineData("",false)]
    public void CreateInvalidVO(string input, bool expectedFalse)
    {
        var invalidVO = SimpleValueObject.From(input);
        Assert.Equal(expectedFalse, invalidVO.ValidationStatus.IsValid);
    }
}
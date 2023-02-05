// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections;
using System.Collections.Generic;
using DFlow.BusinessObjects;
using DFlow.Testing.Supporting.DomainObjects;
using Ecommerce.Domain;

namespace DFlow.Testing.Supporting.DataProviders;

public class PrimaryEntityForAggregate : IEnumerable<object[]>
{
    static SecondaryEntity secondary =  SecondaryEntity.From(
        SecondaryEntityId.NewId(),
        SimpleValueObject.From("secondary"),
        VersionId.New());
    private static SimpleValueObject simpleValue = SimpleValueObject.From("name");
    
    private readonly List<object[]> _data = new()
    {
        new object[]
        {
            secondary
            , simpleValue
            , PrimaryEntity.NewEntity(secondary,simpleValue)
        }
    };

    public IEnumerator<object[]> GetEnumerator()
    {
        return this._data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
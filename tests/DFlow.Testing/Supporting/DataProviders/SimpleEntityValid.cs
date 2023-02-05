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

namespace DFlow.Testing.Supporting.DataProviders;

public class SimpleEntityValid : IEnumerable<object[]>
{
    private static Guid entityId = Guid.NewGuid();

    private static readonly SimpleEntity entity = SimpleEntity.From(
        SimpleEntityId.From(entityId),
        SimpleValueObject.From("name"),
        VersionId.New()
    );

    private readonly List<object[]> _data = new()
    {
        new object[]
        {
            SimpleEntityId.From(entityId), SimpleValueObject.From("name"), VersionId.New(), entity
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
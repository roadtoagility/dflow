// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using DFlow.BusinessObjects;
using DFlow.Validation;

namespace DFlow.Testing.Supporting.DomainObjects;

public sealed class SimpleValueObject : ValueOf<string, SimpleValueObject>
{
    private static readonly string ValueEmpty = string.Empty;

    public static SimpleValueObject Empty()
    {
        return From(ValueEmpty);
    }

    protected override void Validate()
    {
        if (string.IsNullOrEmpty(Value) ||
            string.IsNullOrWhiteSpace(Value))
        {
            ValidationStatus.Append(Failure.For("Value","The simple value can not be empty"));
        }
    }
}
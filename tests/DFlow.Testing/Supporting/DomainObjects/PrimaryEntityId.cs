// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using DFlow.BusinessObjects;
using DFlow.Validation;

namespace DFlow.Testing.Supporting.DomainObjects;

public class PrimaryEntityId : ValueOf<Guid, PrimaryEntityId>
{
    public static PrimaryEntityId Empty => From(Guid.Empty);

    public static PrimaryEntityId From(string inputId)
    {
        var id = Guid.Empty;
        if (Guid.TryParse(inputId, out id) == false)
        {
            var result = From(Empty.Value);
            result.ValidationStatus.Append(Failure.For("Identity", $"The Simple entity id is not valid.", inputId));
            return result;
        }

        return From(id);
    }

    public static PrimaryEntityId NewId()
    {
        return From(Guid.NewGuid());
    }
}
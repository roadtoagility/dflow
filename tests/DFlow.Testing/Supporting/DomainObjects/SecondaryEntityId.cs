// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using DFlow.BusinessObjects;

namespace DFlow.Testing.Supporting.DomainObjects;

public class SecondaryEntityId : ValueOf<Guid, SecondaryEntityId>
{
    public static SecondaryEntityId Empty => From(Guid.Empty);

    public static SecondaryEntityId NewId()
    {
        return From(Guid.NewGuid());
    }
}
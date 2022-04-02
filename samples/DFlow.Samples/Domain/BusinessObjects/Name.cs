﻿// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.Domain.BusinessObjects;

namespace DFlow.Samples.Domain.BusinessObjects
{
    public sealed class Name : ValueOf<string, Name>
    {
        public static Name Empty()
        {
            return From(string.Empty);
        }
    }
}
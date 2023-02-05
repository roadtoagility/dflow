// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;

namespace DFlow.BusinessObjects
{
    public sealed class EntityId : ValueOf<Guid, EntityId>
    {
        public static EntityId Empty()
        {
            return From(Guid.Empty);
        }

        public static EntityId New()
        {
            return From(Guid.NewGuid());
        }
    }
}
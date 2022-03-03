// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Validation;
using DFlow.Samples.BusinessObjects.Domain.BusinessObjects.Validations;

namespace DFlow.Samples.Domain.BusinessObjects
{
    public sealed class EntityId : ValueOf<Guid,EntityId, EntityIdValidator>
    {
        public static EntityId Empty()
        {
            return From(Guid.Empty);
        }
        
        public static EntityId GetNext()
        {
            return From(Guid.NewGuid());
        }
    }
}
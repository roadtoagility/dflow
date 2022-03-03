// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Validation;
using DFlow.Tests.Supporting.DomainObjects.Validators;

namespace DFlow.Tests.Supporting.DomainObjects
{
    public sealed class EntityTestId : ValueOf<Guid,EntityTestId, EntityIdTestValidator>
    {
       private static readonly Guid EmptyId = Guid.Empty;
       
        public static EntityTestId Empty()
        {
            return From(EmptyId);
        }
        
        public static EntityTestId GetNext()
        {
            return From(Guid.NewGuid());
        }
    }
}
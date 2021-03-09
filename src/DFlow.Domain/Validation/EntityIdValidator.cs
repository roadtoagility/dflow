// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using DFlow.Domain.BusinessObjects;
using FluentValidation;

namespace DFlow.Domain.Validation
{
    public sealed class EntityIdValidator: AbstractValidator<EntityId>
    {
        public EntityIdValidator()
        {
            RuleFor(id => id.Value).NotNull();
            RuleFor(id => id.Value).NotEqual(Guid.Empty);
        }
    }
}
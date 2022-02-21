// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using FluentValidation;

namespace DFlow.Tests.Domain.DomainObjects.Validators
{
    public sealed class EntityIdTestValidator: AbstractValidator<EntityTestId>
    {
        public EntityIdTestValidator()
        {
            RuleFor(id => id.Value).NotNull();
            RuleFor(id => id.Value).NotEqual(Guid.Empty);
        }
    }
}
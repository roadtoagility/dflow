// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using FluentValidation;

namespace DFlow.Samples.BusinessObjects.Domain.BusinessObjects.Validations
{
    public sealed class NameValidator: AbstractValidator<Name>
    {
        public NameValidator()
        {
            RuleFor(name => name.Value).NotNull();
            RuleFor(name => name.Value).NotEmpty();
        }
    }
}
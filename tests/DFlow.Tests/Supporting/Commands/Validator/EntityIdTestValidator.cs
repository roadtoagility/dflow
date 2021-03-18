// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.Domain.Validation;
using FluentValidation;

namespace DFlow.Tests.Supporting.Commands.Validator
{
    public sealed class AddEntityCommandValidator: BaseCommandValidator<AddEntityCommand>
    {
        public AddEntityCommandValidator()
        {
            RuleFor(et => et.Name).NotNull();
            RuleFor(et => et.Name).NotEmpty();
            RuleFor(et => et.Mail).EmailAddress();
        }
    }
}
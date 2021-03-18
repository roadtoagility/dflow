// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using DFlow.Samples.Domain.BusinessObjects;
using FluentValidation;

namespace DFlow.Samples.BusinessObjects.Domain.BusinessObjects.Validations
{
    public sealed class UserValidator: AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Id).SetValidator(new EntityIdValidator());
            RuleFor(user => user.Name).SetValidator(new NameValidator());
            RuleFor(user => user.Mail).SetValidator(new EmailValidator());
        }
    }
}
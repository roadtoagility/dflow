// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.Tests.Supporting.DomainObjects.Validators;
using FluentValidation;

namespace DFlow.Tests.Supporting.DomainObjects
{
    public class BusinessEntityValidator: AbstractValidator<BusinessEntity>
    {
        public BusinessEntityValidator()
        {
            RuleFor(id => id.BusinessTestId).NotNull();
            RuleFor(obj => obj.Email).SetValidator(new EmailValidator());
        }
    }
}
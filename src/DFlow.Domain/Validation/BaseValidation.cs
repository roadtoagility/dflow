// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Collections.Generic;
using System.Collections.Immutable;
using DFlow.Domain.BusinessObjects;
using FluentValidation.Results;

namespace DFlow.Domain.Validation
{
    public abstract class BaseValidation: IValidable
    {
        private readonly List<ValidationFailure> _failures = new List<ValidationFailure>();
        public void AppendValidationResult(ValidationResult result)
        {
            _failures.AddRange(result.Errors);
        }

        public IReadOnlyList<ValidationFailure> Failures => _failures.ToImmutableList();

        public bool IsValid => _failures.Count == 0;
    }
}
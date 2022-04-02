// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Collections.Generic;
using System.Collections.Immutable;

namespace DFlow.Domain.Validation
{
    public abstract class BaseValidation : IValidable
    {
        private readonly List<Failure> _failures = new();

        public IReadOnlyList<Failure> Failures => _failures.ToImmutableList();

        public void AppendValidationResult(Failure failure)
        {
            _failures.Add(failure);
        }

        public void AppendValidationResult(IReadOnlyList<Failure> failures)
        {
            _failures.AddRange(failures);
        }

        public bool IsValid => _failures.Count == 0;
    }
}
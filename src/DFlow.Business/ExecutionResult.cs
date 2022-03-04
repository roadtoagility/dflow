// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using FluentValidation.Results;

namespace DFlow.Business.Cqrs
{
    public class ExecutionResult
    {
        public ExecutionResult(bool isSucceed, IReadOnlyList<ValidationFailure> violations)
        {
            IsSucceed = isSucceed;
            Violations = violations;
        }

        public bool IsSucceed { get; }

        public IReadOnlyList<ValidationFailure> Violations { get; }
    }
}
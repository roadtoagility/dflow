// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Collections.Generic;

namespace DFlow.Domain.Validation
{
    public interface IValidable
    {
        bool IsValid { get; }
        void AppendValidationResult(Failure failure);

        void AppendValidationResult(IReadOnlyList<Failure> failures);
    }
}
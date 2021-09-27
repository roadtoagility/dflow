// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Collections.Generic;
using FluentValidation.Results;

namespace DFlow.Domain.Validation
{
    public readonly struct ValidationFailure
    {
        public ValidationFailure(string propertyName, string errorTag)
        {
            PropertyName = propertyName;
            ErrorTag = errorTag;
        }
        
        public string PropertyName { get; }
        public string ErrorTag { get; }
    }
}
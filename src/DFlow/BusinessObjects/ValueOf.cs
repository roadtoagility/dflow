// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.Validation;

namespace DFlow.BusinessObjects
{
    public class ValueOf<TValue, TThis> : ValueOf.ValueOf<TValue, TThis>
        where TThis : ValueOf<TValue, TThis>, new()
    {
        private readonly ValidationResult _validationStatus = ValidationResult.Empty();
        public ValidationResult ValidationStatus => _validationStatus;
    }
}
// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using DFlow.Domain.Validation;

namespace DFlow.Domain.BusinessObjects
{
    public sealed class AggregationName : ValidationStatus
    {
        public string Value { get; }

        private AggregationName(string name)
        {
            Value = name;
        }

        public static AggregationName From(string current)
        {
            var aggregationName = new AggregationName(current);
            var validator = new AggregationNameValidator();

            aggregationName.SetValidationResult(validator.Validate(aggregationName));

            return aggregationName;
        }

        public override string ToString()
        {
            return $"{Value}";
        }

        #region IEquatable

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        #endregion

    }
}
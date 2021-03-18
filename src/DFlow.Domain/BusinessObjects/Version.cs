// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using DFlow.Domain.Validation;

namespace DFlow.Domain.BusinessObjects
{
    public sealed class Version : ValidationStatus
    {
        public int Value { get; }

        private Version(int version)
        {
            Value = version;
        }



        public static Version From(int current)
        {
            var version = new Version(current);
            var validator = new VersionValidator();

            version.SetValidationResult(validator.Validate(version));

            return new Version(current);
        }

        public static Version Empty()
        {
            return From(0);
        }

        public static Version New()
        {
            return From(1);
        }

        public static Version Next(Version current)
        {
            return From(current.Value + 1);
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

        public static bool operator >=(Version a, Version b)
            => a.Value >= b.Value;

        public static bool operator <=(Version a, Version b)
            => a.Value <= b.Value;
        
        public static bool operator >(Version a, Version b)
            => a.Value > b.Value;

        public static bool operator <(Version a, Version b)
            => a.Value < b.Value;
    }
}
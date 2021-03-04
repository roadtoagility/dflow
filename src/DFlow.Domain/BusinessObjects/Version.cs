// Copyright (C) 2020  Road to Agility
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Library General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Library General Public License for more details.
//
// You should have received a copy of the GNU Library General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 51 Franklin St, Fifth Floor,
// Boston, MA  02110-1301, USA.
//

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
// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using DFlow.Domain.Validation;
using DFlow.Samples.BusinessObjects.Domain.BusinessObjects.Validations;

namespace DFlow.Samples.BusinessObjects.Domain.BusinessObjects
{
    public sealed class Name : ValidationStatus
    {
        public string Value { get; }
        
        private Name(string name)
        {
            Value = name;
        }

        public static Name From(string name)
        {
            var userName = new Name(name);
            var validator = new NameValidator();

            userName.SetValidationResult(validator.Validate(userName));
            
            return userName;
        }

        public static Name Empty()
        {
            return new Name(String.Empty);
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
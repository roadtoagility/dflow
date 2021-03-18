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
    public sealed class Email : ValidationStatus
    {
        public string Value { get; }
        
        private Email(string name)
        {
            Value = name;
        }

        public static Email From(string name)
        {
            var email = new Email(name);
            var validator = new EmailValidator();

            email.SetValidationResult(validator.Validate(email));
            
            return email;
        }
        
        public static Email Empty()
        {
            return new Email(String.Empty);
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
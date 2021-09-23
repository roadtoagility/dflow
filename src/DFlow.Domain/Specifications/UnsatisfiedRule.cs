// Copyright (C) 2021  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;

namespace DFlow.Domain.Specifications
{
    public readonly struct UnsatisfiedRule
    {
        public string PropertyName { get; }
        public string Message { get; }
        public string Value { get; }
        
        private UnsatisfiedRule(string propertyName, string message, string value)
        {
            PropertyName = propertyName;
            Message = message;
            Value = value;
        }

        public static UnsatisfiedRule For(string propertyName, string message, string value)
        {
            if (string.IsNullOrEmpty(propertyName) == true)
            {
                throw new ArgumentNullException(propertyName);
            }
            
            if (string.IsNullOrEmpty(message) == true)
            {
                throw new ArgumentNullException(message);
            }
            
            return new UnsatisfiedRule(propertyName, message, value);
        }

        public static UnsatisfiedRule For(string propertyName, string message)
        {
            return For(propertyName, message, string.Empty);
        }
    }
}

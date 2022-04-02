// Copyright (C) 2021  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace DFlow.Domain.Validation
{
    public class Failure
    {
        public Failure(string propertyName, string message, string value)
        {
            PropertyName = propertyName;
            Message = message;
            Value = value;
        }

        public string PropertyName { get; }

        public string Message { get; }

        public string Value { get; }

        public static Failure For(string propertyName, string message, string value)
        {
            return new Failure(propertyName, message, value);
        }

        public static Failure For(string propertyName, string message)
        {
            return For(propertyName, message, string.Empty);
        }
    }
}
// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Collections.Generic;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Validation;
using DFlow.Samples.BusinessObjects.BusinessObjects.Validations;

namespace DFlow.Samples.BusinessObjects.BusinessObjects
{
    public sealed class User : ValidationStatus
    {
        private User(EntityId clientId, Name name, Email commercialEmail, Version version)
        {
            Id = clientId;
            Name = name;
            Mail = commercialEmail;
            Version = version;
        }
        public EntityId Id { get; }
        
        public Name Name { get; }
        
        public Email Mail { get; }
        
        public Version Version { get; }

        public bool IsNew() => Version.Value == 1;
                
        public static User From(EntityId clientId, Name name, Email commercialEmail, Version version)
        {
            var user = new User(clientId,name,commercialEmail, version);
            var validator = new UserValidator();
            user.SetValidationResult(validator.Validate(user));
            return user;        
        }

        public static User Empty()
        {
            return default(User);
        }
        
        public override string ToString()
        {
            return $"[User]:[ID: {Id} Name: {Name}, Commercial Email: {Mail}]";
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
            yield return Mail;
        }
    }
}
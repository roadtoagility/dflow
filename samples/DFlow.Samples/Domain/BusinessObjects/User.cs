// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Collections.Generic;
using System.Collections.Immutable;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Validation;
using DFlow.Samples.BusinessObjects.Domain.BusinessObjects;
using DFlow.Samples.BusinessObjects.Domain.BusinessObjects.Validations;

namespace DFlow.Samples.Domain.BusinessObjects
{
    public sealed class User : BaseEntity<EntityId>
    {
        private User(EntityId clientId, Name name, Email commercialEmail, VersionId version)
        :base(clientId,version)
        {
            Name = name;
            Mail = commercialEmail;
            
            AppendValidationResult(Identity.ValidationStatus.Errors.ToImmutableList());
            AppendValidationResult(Name.ValidationStatus.Errors.ToImmutableList());
            AppendValidationResult(Mail.ValidationStatus.Errors.ToImmutableList());
        }
        public Name Name { get; }
        
        public Email Mail { get; }
                
        public static User From(EntityId clientId, Name name, Email commercialEmail, VersionId version)
        {
            var user = new User(clientId,name,commercialEmail,version);
            return user;
        }

        public static User Empty()
        {
            return From(EntityId.Empty(), Name.Empty(), Email.Empty(), VersionId.Empty());
        }
        
        public override string ToString()
        {
            return $"[User]:[ID: {Identity} Name: {Name}, Commercial Email: {Mail}]";
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Identity;
            yield return Name;
            yield return Mail;
            yield return Version;
        }
    }
}
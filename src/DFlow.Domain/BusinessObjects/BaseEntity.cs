// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System;
using System.Collections.Generic;
using System.Linq;
using DFlow.Domain.Validation;

namespace DFlow.Domain.BusinessObjects
{
    public abstract class BaseEntity<TIdentity> : BaseValidation,
        IEntityIdentity<TIdentity>
    {
        protected BaseEntity(TIdentity identity, VersionId version)
        {
            Identity = identity;
            Version = version;
        }

        public VersionId Version { get; }

        public TIdentity Identity { get; }

        public bool IsNew()
        {
            return Version.Initial;
        }

        public override string ToString()
        {
            return $"[ENTITY]:[IDENTITY: {Identity}]";
        }

        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (GetType() != obj.GetType()) return false;

            var entity = (BaseEntity<TIdentity>)obj;

            return GetEqualityComponents().SequenceEqual(entity.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(GetEqualityComponents());
        }
    }
}
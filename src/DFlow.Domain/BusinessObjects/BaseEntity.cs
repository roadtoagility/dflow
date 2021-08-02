// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Collections.Generic;
using DFlow.Domain.Validation;
using FluentValidation.Results;

namespace DFlow.Domain.BusinessObjects
{
    public abstract class BaseEntity<TIdentity>: BaseValidation, 
        IEntityIdentity<TIdentity>
    {
        protected BaseEntity(TIdentity identity, VersionId version)
        {
            Identity = identity;
            Version = version;
        }
        
        public TIdentity Identity { get; }
        
        public VersionId Version { get; }

        public bool IsNew() => Version.Initial;
        
        public override string ToString()
        {
            return $"[ENTITY]:[IDENTITY: {Identity}]";
        }
    }
}
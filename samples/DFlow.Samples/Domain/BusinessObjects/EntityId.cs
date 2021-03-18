// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Validation;
using DFlow.Samples.BusinessObjects.Domain.BusinessObjects.Validations;

namespace DFlow.Samples.Domain.BusinessObjects
{
    public sealed class EntityId : ValidationStatus, IIdentity<Guid>
    {
        public Guid Value { get; }
        
        private EntityId(Guid id)
        {
            Value = id;
        }

        public static EntityId From(Guid id)
        {
            var entityId = new EntityId(id);
            var validator = new EntityIdValidator();

            entityId.SetValidationResult(validator.Validate(entityId));
            
            return entityId;
        }
        
        public static EntityId Empty()
        {
            return EntityId.From(Guid.Empty);
        }
        
        public static EntityId GetNext()
        {
            return From(Guid.NewGuid());
        }
        public override string ToString()
        {
            return Value.ToString("N");
        }

        #region IEquatable

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        #endregion

        #region IComparable

        public int CompareTo(EntityId other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }

            return Value.CompareTo(other.Value);
        }

        #endregion
    }
}
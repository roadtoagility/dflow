// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using DFlow.Domain.Validation;
using DFlow.Tests.Domain.DomainObjects.Validators;

namespace DFlow.Tests.Domain.DomainObjects
{
    public sealed class EntityTestId : ValidationStatus
    {
        public Guid Value { get; }
        
        private EntityTestId(Guid id)
        {
            Value = id;
        }

        public static EntityTestId From(Guid id)
        {
            var entityId = new EntityTestId(id);
            var validator = new EntityIdTestValidator();

            entityId.SetValidationResult(validator.Validate(entityId));
            
            return entityId;
        }
        
        public static EntityTestId Empty()
        {
            return EntityTestId.From(Guid.Empty);
        }
        
        public static EntityTestId GetNext()
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

        public int CompareTo(EntityTestId other)
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
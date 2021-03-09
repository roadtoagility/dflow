// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using DFlow.Domain.Aggregates;
using DFlow.Domain.Validation;
using DFlow.Tests.Supporting.DomainObjects.Events;
using FluentValidation;
using Version = DFlow.Domain.BusinessObjects.Version;

namespace DFlow.Tests.Supporting.DomainObjects
{
    public class BusinessEntity : ValidationStatus
    {
        private BusinessEntity(EntityTestId businessTestId, Version version)
        {
            BusinessTestId = businessTestId;
            Version = version;
        }

        public EntityTestId BusinessTestId { get; }

        public Version Version { get; }

        public bool IsNew() => Version.Value == 1; 

        public static BusinessEntity From(EntityTestId testId, Version version)
        {
            var bobj = new BusinessEntity(testId, version);
            var validator = new BusinessEntityValidator();
            bobj.SetValidationResult(validator.Validate(bobj));
            
            return bobj;
        }
        
        public static BusinessEntity New()
        {
            return From(EntityTestId.GetNext(), Version.New());
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return BusinessTestId;
            yield return Version;
        }
    }

    public class BusinessEntityValidator: AbstractValidator<BusinessEntity>
    {
        public BusinessEntityValidator()
        {
            RuleFor(id => id.BusinessTestId).NotNull();            
        }
    }
    
    public sealed class BusinessEntityAggregateRoot:AggregationRoot<BusinessEntity>
    {
        private BusinessEntityAggregateRoot(BusinessEntity businessEntity)
        {
            if (businessEntity.ValidationResults.IsValid)
            {
                Apply(businessEntity);

                if (businessEntity.IsNew())
                {
                    Raise(EntityAddedEvent.For(businessEntity));                    
                }
            }

            ValidationResults = businessEntity.ValidationResults;
        }

        public static BusinessEntityAggregateRoot Create()
        {
            return new BusinessEntityAggregateRoot(BusinessEntity.New());
        }
        
        public static BusinessEntityAggregateRoot ReconstructFrom(BusinessEntity entity)
        {
            return new BusinessEntityAggregateRoot(BusinessEntity.From(entity.BusinessTestId, Version.Next(entity.Version)));
        }
    }
}
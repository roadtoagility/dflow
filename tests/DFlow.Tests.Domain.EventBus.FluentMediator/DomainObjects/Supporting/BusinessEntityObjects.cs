// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using DFlow.Domain.Aggregates;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Validation;
using FluentValidation;
using Version = DFlow.Domain.BusinessObjects.Version;

namespace DFlow.Tests.Domain.DomainObjects.Supporting
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
    
    public sealed class TestAggregateRoot:AggregationRoot<BusinessEntity>
    {
        private TestAggregateRoot(BusinessEntity businessEntity)
        {
            if (businessEntity.ValidationResults.IsValid)
            {
                Apply(businessEntity);
            }

            ValidationResults = businessEntity.ValidationResults;
        }

        public static TestAggregateRoot Create()
        {
            return new TestAggregateRoot(BusinessEntity.New());
        }
        
        public static TestAggregateRoot ReconstructFrom(BusinessEntity entity)
        {
            return new TestAggregateRoot(BusinessEntity.From(entity.BusinessTestId, Version.Next(entity.Version)));
        }
    }
}
// Copyright (C) 2020  Road to Agility
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Library General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Library General Public License for more details.
//
// You should have received a copy of the GNU Library General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 51 Franklin St, Fifth Floor,
// Boston, MA  02110-1301, USA.
//

using System.Collections.Generic;
using DFlow.Domain.Aggregates;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Validation;
using FluentValidation;
using Version = DFlow.Domain.BusinessObjects.Version;

namespace DFlow.Tests.Domain.BusinessObjects.Supporting
{
    public class BusinessEntity : ValidationStatus
    {
        private BusinessEntity(EntityId businessId, Version version)
        {
            BusinessId = businessId;
            Version = version;
        }

        public EntityId BusinessId { get; }

        public Version Version { get; }

        public bool IsNew() => Version.Value == 1; 

        public static BusinessEntity From(EntityId id, Version version)
        {
            var bobj = new BusinessEntity(id, version);
            var validator = new BusinessEntityValidator();
            bobj.SetValidationResult(validator.Validate(bobj));
            
            return bobj;
        }
        
        public static BusinessEntity New()
        {
            return From(EntityId.GetNext(), Version.New());
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return BusinessId;
            yield return Version;
        }
    }

    public class BusinessEntityValidator: AbstractValidator<BusinessEntity>
    {
        public BusinessEntityValidator()
        {
            RuleFor(id => id.BusinessId).NotNull();            
        }
    }
    
    public sealed class TestAggregateRoot:AggregationRoot<BusinessEntity>
    {
        private TestAggregateRoot(BusinessEntity businessEntity)
        {
            if (businessEntity.ValidationResults.IsValid)
            {
                Apply(businessEntity);
                
                if (businessEntity.IsNew())
                {
                    // Raise(ProjectAddedEvent.For(project));
                }
            }

            ValidationResults = businessEntity.ValidationResults;
        }
        
        // private TestAggregateRoot(EntityId id, ProjectName name, ProjectCode code, 
        //     Money budget, DateAndTime startDate, EntityId clientId)
        //     : this(Project.NewRequest(id, name,code,startDate,budget,clientId))
        // {
        // }

        public static TestAggregateRoot Create()
        {
            return new TestAggregateRoot(BusinessEntity.New());
        }
        
        public static TestAggregateRoot ReconstructFrom(BusinessEntity entity)
        {
            return new TestAggregateRoot(BusinessEntity.From(entity.BusinessId, Version.Next(entity.Version)));
        }
    }
}
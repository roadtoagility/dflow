using System.Collections.Immutable;
using DFlow.Domain.Aggregates;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Events;
using DFlow.Tests.Supporting.DomainObjects.Events;

namespace DFlow.Tests.Supporting.DomainObjects
{
    public sealed class BusinessEntityAggregateRoot:ObjectBasedAggregationRootWithEvents<BusinessEntity, EntityTestId>
    {
        internal BusinessEntityAggregateRoot(BusinessEntity businessEntity)
        {
            if (businessEntity.IsValid)
            {
                Apply(businessEntity);

                if (businessEntity.IsNew())
                {
                    Raise(EntityAddedEvent.For(businessEntity));                    
                }
            }

            AppendValidationResult(businessEntity.Failures);
        }

        public static BusinessEntityAggregateRoot Create()
        {
            return new BusinessEntityAggregateRoot(BusinessEntity.New());
        }
        
        public static BusinessEntityAggregateRoot ReconstructFrom(BusinessEntity entity)
        {
            return new BusinessEntityAggregateRoot(BusinessEntity.From(entity.Identity, VersionId.Next(entity.Version)));
        }
    }
}
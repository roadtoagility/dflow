using DFlow.Domain.Aggregates;
using DFlow.Domain.BusinessObjects;
using DFlow.Tests.Supporting.DomainObjects.Events;

namespace DFlow.Tests.Supporting.DomainObjects
{
    public sealed class BusinessEntityAggregateRoot:ObjectBasedAggregationRoot<BusinessEntity>
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
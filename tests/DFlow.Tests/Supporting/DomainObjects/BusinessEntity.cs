using System.Collections.Generic;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Validation;

namespace DFlow.Tests.Supporting.DomainObjects
{
    public class BusinessEntity : BaseEntity<EntityTestId>
    {
        private BusinessEntity(EntityTestId businessTestId, VersionId version)
            :base(businessTestId,version)
        {
        }

        public static BusinessEntity From(EntityTestId testId, VersionId version)
        {
            var bobj = new BusinessEntity(testId, version);
            return bobj;
        }
        
        public static BusinessEntity New()
        {
            return From(EntityTestId.GetNext(), VersionId.New());
        }
    }
}
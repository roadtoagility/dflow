using System.Collections.Generic;
using DFlow.Domain.BusinessObjects;

namespace DFlow.Tests.Supporting.DomainObjects
{
    public class BusinessEntity : BaseEntity<EntityTestId>
    {
        private BusinessEntity(EntityTestId businessTestId, VersionId version)
            : base(businessTestId, version)
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

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Identity;
            yield return Version;
        }
    }
}
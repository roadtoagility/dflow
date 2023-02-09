using System.Collections.Generic;
using DFlow.BusinessObjects;

namespace DFlow.Testing.Supporting.DomainObjects
{
    public class BusinessEntity : EntityBase<EntityTestId>
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

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Identity;
            yield return Version;
        }
    }
}
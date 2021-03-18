using System.Collections.Generic;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Validation;

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
}
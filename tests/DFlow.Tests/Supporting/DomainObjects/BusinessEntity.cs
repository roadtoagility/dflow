using System.Collections.Generic;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Validation;
using DFlow.Tests.Supporting.DomainObjects.Validators;

namespace DFlow.Tests.Supporting.DomainObjects
{
    public class BusinessEntity : ValidationStatus
    {
        public  Email Email { get; private set; }

        private BusinessEntity(EntityTestId businessTestId, Version version)
        {
            BusinessTestId = businessTestId;
            Version = version;
        }

        private BusinessEntity(EntityTestId businessTestId, Version version, Email email)
            : this(businessTestId, version)
        {
            Email = email;
        }

        public EntityTestId BusinessTestId { get; }

        public Version Version { get; }

        public bool IsNew() => Version.Value == 1; 

        public static BusinessEntity From(EntityTestId testId, Version version)
        {
            var bobj = From(testId, null, version);
            return bobj;
        }

        public static BusinessEntity From(EntityTestId testId, Email email, Version version)
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
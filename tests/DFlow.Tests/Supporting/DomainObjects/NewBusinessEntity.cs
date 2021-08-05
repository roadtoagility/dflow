using System.Collections.Generic;
using System.Collections.Immutable;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Validation;

namespace DFlow.Tests.Supporting.DomainObjects
{
    public class NewBusinessEntity : BaseEntity<NewEntityTestId>
    {
        public static NewBusinessEntity From(NewEntityTestId testId, VersionId version)
        {
            return new NewBusinessEntity(testId, version);
        }
        
        public static NewBusinessEntity New()
        {
            return From(NewEntityTestId.GetNext(), VersionId.New());
        }
        private NewBusinessEntity(NewEntityTestId businessTestId, VersionId version)
        :base(businessTestId, version)
        {
            AppendValidationResult(businessTestId.ValidationStatus.Errors.ToImmutableList());
            AppendValidationResult(version.ValidationStatus.Errors.ToImmutableList());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Identity;
            yield return Version;
        }
    }
}
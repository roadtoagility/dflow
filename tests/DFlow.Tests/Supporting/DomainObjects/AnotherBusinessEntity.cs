using System.Collections.Generic;
using DFlow.Domain.BusinessObjects;

namespace DFlow.Tests.Supporting.DomainObjects
{
    public sealed class AnotherBusinessEntity : BaseEntity<EntityTestId>
    {
        private AnotherBusinessEntity(EntityTestId businessTestId, VersionId version, Name entityName,
            Email entityEmail)
            : base(businessTestId, version)
        {
            EntityEmail = entityEmail;
            EntityName = entityName;
        }

        public Name EntityName { get; }
        public Email EntityEmail { get; }

        public static AnotherBusinessEntity From(EntityTestId testId, Name entityName, Email entityEmail,
            VersionId version)
        {
            var bobj = new AnotherBusinessEntity(testId, version, entityName, entityEmail);
            return bobj;
        }

        public static AnotherBusinessEntity New(Name entityName, Email entityEmail)
        {
            return From(EntityTestId.GetNext(), entityName, entityEmail, VersionId.New());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Identity;
            yield return EntityName;
            yield return EntityEmail;
            yield return Version;
        }
    }
}
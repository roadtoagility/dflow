using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using DFlow.Base;
using DFlow.Bus;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Specifications;
using DFlow.Example;
using DFlow.Example.Aggregates;
using DFlow.Example.Commands;
using DFlow.Example.Events;
using DFlow.Example.Views;
using DFlow.Tests.Supporting.DomainObjects;
using DFlow.Tests.Supporting.Specifications;
using Xunit;

namespace DFlow.Tests
{
    public class SpecificationForValidationReportsTests
    {
        [Fact]
        public void RetrieveAggregateLoadedFromSnapshot()
        {
            
        }
    }
    
    public class SpecificationForCreationAggregationTests
    {
        [Fact]
        public void RetrieveAggregateLoadedFromSnapshot()
        {

        }
    }
    
    public class SpecificationForCreationEntityTests
    {
        [Fact]
        public void BusinessEntityIsNew()
        {
            var bu = BusinessEntity.New();
            var isNew = new BusinessEntityIsNew();

            Assert.True(isNew.IsSatisfiedBy(bu));
        }
        
        [Fact]
        public void BusinessEntityNotIsNew()
        {
            var buUpdated = BusinessEntity.From(EntityTestId.GetNext(), VersionId.Next(VersionId.New()));
            var isNew = new BusinessEntityIsNew();

            Assert.False(isNew.IsSatisfiedBy(buUpdated));
        }
        
        [Fact]
        public void AnotherBusinessEntityNameIsRoad()
        {
            var bu = AnotherBusinessEntity
                .New(Name.From("Road"), Email.From("my@email.com"));
            
            var isRoad = new AnotherBusinessEntityNameIsRoad(Name.From("Road"));

            Assert.True(isRoad.IsSatisfiedBy(bu));
        }
        
        [Fact]
        public void AnotherBusinessEntityNameIsRoadAndEmailFromRoadCompany()
        {
            var bu = AnotherBusinessEntity
                .New(Name.From("Road"), Email.From("email@roadtoagility.com"));
            
            var isRoad = new AnotherBusinessEntityNameIsRoad(Name.From("Road"));
            var isFromCompany = new AnotherBusinessEntityEmailFromCompany(Email.From("email@roadtoagility.com"));

            isRoad.And(isFromCompany);
            
            Assert.True(isRoad.IsSatisfiedBy(bu));
        }
    }
}
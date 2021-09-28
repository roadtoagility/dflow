// Copyright (C) 2021  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using DFlow.Domain.BusinessObjects;
using DFlow.Tests.Supporting.DomainObjects;
using DFlow.Tests.Supporting.Specifications;
using Xunit;

namespace DFlow.Tests.Domain
{
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
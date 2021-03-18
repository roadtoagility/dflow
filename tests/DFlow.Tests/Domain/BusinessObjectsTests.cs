// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System;
using AutoFixture;
using DFlow.Tests.Supporting.DomainObjects;
using Xunit;
using Version = DFlow.Domain.BusinessObjects.Version;

namespace DFlow.Tests.Domain
{
    public sealed class BusinessObjectsTests
    {
        [Fact]
        public void EntityId_create_a_valid()
        {
            var fixture = new Fixture();
            fixture.Register<EntityTestId>(() => EntityTestId.From(fixture.Create<Guid>()));

            var entityId = fixture.Create<EntityTestId>();
            
            Assert.True(entityId.ValidationResults.IsValid);
        }
        
        [Fact]
        public void EntityId_create_an_empty()
        {
            var fixture = new Fixture();
            fixture.Register<EntityTestId>(() => EntityTestId.Empty());

            var entityId = fixture.Create<EntityTestId>();
            
            Assert.False(entityId.ValidationResults.IsValid);
        }
        
        [Fact]
        public void EntityId_create_an_invalid()
        {
            var fixture = new Fixture();
            fixture.Register<EntityTestId>(() => EntityTestId.From(Guid.Empty));

            var entityId = fixture.Create<EntityTestId>();
            
            Assert.False(entityId.ValidationResults.IsValid);
        }
        
        [Fact]
        public void Version_create_a_valid_from()
        {
            var fixture = new Fixture();
            var input = fixture.Create<int>();
            fixture.Register<Version>(() => Version.From(input));

            var version = fixture.Create<Version>();
            
            Assert.Equal(input,version.Value);
        }
        
        [Fact]
        public void Version_create_an_empty()
        {
            var entityId = Version.Empty();
            
            Assert.True(entityId.Equals(Version.Empty()));
        }

        [Fact]
        public void Version_get_next()
        {
            var version = Version.New();
            var next = Version.Next(version);
            var nextCheck = version.Value + 1;
            Assert.Equal(next.Value,nextCheck);
        }
    }
}
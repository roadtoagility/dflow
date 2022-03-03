// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System;
using AutoFixture;
using DFlow.Domain.BusinessObjects;
using DFlow.Tests.Supporting.DomainObjects;
using Xunit;

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
            
            Assert.True(entityId.ValidationStatus.IsValid);
        }
        
        [Fact]
        public void EntityId_create_an_empty()
        {
            var fixture = new Fixture();
            fixture.Register<EntityTestId>(() => EntityTestId.Empty());

            var entityId = fixture.Create<EntityTestId>();
            
            Assert.False(entityId.ValidationStatus.IsValid);
        }
        
        [Fact]
        public void EntityId_create_an_invalid()
        {
            var fixture = new Fixture();
            fixture.Register<EntityTestId>(() => EntityTestId.From(Guid.Empty));

            var entityId = fixture.Create<EntityTestId>();
            
            Assert.False(entityId.ValidationStatus.IsValid);
        }
        
        [Fact]
        public void Version_create_a_valid_from()
        {
            var fixture = new Fixture();
            var input = fixture.Create<int>();
            fixture.Register<VersionId>(() => VersionId.From(input));

            var version = fixture.Create<VersionId>();
            
            Assert.Equal(input,version.Value);
        }
        
        [Fact]
        public void Version_create_an_empty()
        {
            var entityId = VersionId.Empty();
            
            Assert.True(entityId.Equals(VersionId.Empty()));
        }

        [Fact]
        public void Version_get_next()
        {
            var version = VersionId.New();
            var next = VersionId.Next(version);
            var nextCheck = version.Value + 1;
            Assert.Equal(next.Value,nextCheck);
        }
        
        [Fact]
        public void Version_get_gr_and_lt()
        {
            var version = VersionId.From(1);
            var next = VersionId.Next(version);
            Assert.True(version < next);
            Assert.True(next > version);
        }
    }
}
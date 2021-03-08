// Copyright (C) 2020  Road to Agility
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Library General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Library General Public License for more details.
//
// You should have received a copy of the GNU Library General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 51 Franklin St, Fifth Floor,
// Boston, MA  02110-1301, USA.
//

using System;
using AutoFixture;
using DFlow.Domain.BusinessObjects;
using Xunit;
using Version = DFlow.Domain.BusinessObjects.Version;

namespace DFlow.Tests.Domain.DomainObjects
{
    public sealed class BusinessObjectsTests
    {
        [Fact]
        public void EntityId_create_a_valid()
        {
            var fixture = new Fixture();
            fixture.Register<EntityId>(() => EntityId.From(fixture.Create<Guid>()));

            var entityId = fixture.Create<EntityId>();
            
            Assert.True(entityId.ValidationResults.IsValid);
        }
        
        [Fact]
        public void EntityId_create_an_empty()
        {
            var fixture = new Fixture();
            fixture.Register<EntityId>(() => EntityId.Empty());

            var entityId = fixture.Create<EntityId>();
            
            Assert.False(entityId.ValidationResults.IsValid);
        }
        
        [Fact]
        public void EntityId_create_an_invalid()
        {
            var fixture = new Fixture();
            fixture.Register<EntityId>(() => EntityId.From(Guid.Empty));

            var entityId = fixture.Create<EntityId>();
            
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
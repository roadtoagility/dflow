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
using Version = DFlow.Domain.BusinessObjects.Version;

namespace DFlow.Tests.Domain
{
    public sealed class EntityTests
    {
        [Fact]
        public void EntityId_create_a_valid()
        {
            var fixture = new Fixture();
            fixture.Register<NewBusinessEntity>(() => NewBusinessEntity.New());

            var entity = fixture.Create<NewBusinessEntity>();
            
            Assert.True(entity.IsValid);
        }
        
        [Fact]
        public void EntityId_create_is_not_valid()
        {
            var fixture = new Fixture();
            fixture.Register<NewBusinessEntity>(() => NewBusinessEntity.From(NewEntityTestId.Empty(), VersionId.Empty()));

            var entity = fixture.Create<NewBusinessEntity>();
            
            Assert.False(entity.IsValid);
            Assert.True(entity.Failures.Count == 2);
        }
    }
}
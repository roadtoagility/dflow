// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using AutoFixture;
using DFlow.Domain.BusinessObjects;
using Xunit;
using Version = DFlow.Domain.BusinessObjects.Version;

namespace DFlow.Tests.Domain.DomainEvents
{
    public sealed class DomainEventsTests
    {
        [Fact]
        public void EntityId_create_a_valid()
        {
            var fixture = new Fixture();
            fixture.Register<EntityId>(() => EntityId.From(fixture.Create<Guid>()));

            var entityId = fixture.Create<EntityId>();
            
            Assert.True(entityId.ValidationResults.IsValid);
        }
    }
}
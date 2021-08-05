// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.DomainEvents;
using DFlow.Domain.Events;
using DFlow.Tests.Supporting.DomainObjects;
using Xunit;

namespace DFlow.Tests.Domain
{
    public sealed class AggregatesTests
    {
        [Fact]
        public void Aggregate_create_a_valid()
        {
            var agg = BusinessEntityAggregateRoot.Create();

            Assert.True(agg.IsValid);
        }
        
        [Fact]
        public void Aggregate_reconstruct_a_valid()
        {
            var be = BusinessEntity.From(EntityTestId.GetNext(), VersionId.New());
            var agg = BusinessEntityAggregateRoot.ReconstructFrom(be);

            Assert.True(agg.IsValid);
        }
        
        [Fact]
        public void Aggregate_EventBased_create_a_valid()
        {
            var fixture = new Fixture()
                .Customize(new AutoNSubstituteCustomization{ ConfigureMembers = true });
            fixture.Register<Name>(()=> Name.From(fixture.Create<String>()));
            fixture.Register<Email>(()=> Email.From("email@de.com"));
            
            var name = fixture.Create<Name>();
            var email = fixture.Create<Email>();
            var agg = EventStreamBusinessEntityAggregateRoot.Create(EntityTestId.GetNext(), name, email);
            Assert.Equal(nameof(EventStreamBusinessEntityAggregateRoot),agg.GetChange().Name.Value);
            Assert.True(agg.IsValid);
        }
        
        [Fact]
        public void Aggregate_EventBased_create_an_invalid()
        {
            var name = Name.Empty();
            var email = Email.Empty();
            var agg = EventStreamBusinessEntityAggregateRoot.Create(EntityTestId.GetNext(), name, email);
            Assert.False(agg.IsValid);
        }
        
        [Fact]
        public void Aggregate_EventBased_valid_Entity_create()
        {
            var fixture = new Fixture()
                .Customize(new AutoNSubstituteCustomization{ ConfigureMembers = true });
            fixture.Register<Name>(()=> Name.From(fixture.Create<String>()));
            fixture.Register<Email>(()=> Email.From("email@de.com"));
            
            var name = fixture.Create<Name>();
            var email = fixture.Create<Email>();
            var agg = EventStreamBusinessEntityAggregateRoot.Create(EntityTestId.GetNext(), name, email);
            var change = agg.GetChange();
            Assert.True(agg.IsValid);
            Assert.True(change.IsValid);
            Assert.Equal(1,change.Events.Count);
        }
    }
}
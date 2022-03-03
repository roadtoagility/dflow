// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using DFlow.Domain.Events;
using NSubstitute;
using Xunit;

namespace DFlow.Tests.Domain
{
    public sealed class DomainEventsTests
    {
        [Fact]
        public async Task DomainEvent_Publishing()
        {
            var fixture = new Fixture()
                .Customize(new AutoNSubstituteCustomization{ ConfigureMembers = true });

            var realEventBus = fixture.Create<IDomainEventBus>();
            var myEvent = fixture.Create<IDomainEvent>();
            await realEventBus.Publish(myEvent);
            
            await realEventBus.Received().Publish(Arg.Any<IDomainEvent>());
        }
    }
}
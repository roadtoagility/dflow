// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using AutoFixture;
using AutoFixture.AutoNSubstitute;
using DFlow.Domain.Events;
using DFlow.Tests.Supporting;
using DFlow.Tests.Supporting.Commands;
using DFlow.Tests.Supporting.DomainObjects.Events;
using NSubstitute;
using Xunit;

namespace DFlow.Tests.Business.Cqrs
{
    public sealed class CommandHandlerTests
    {
        [Fact]
        public void Add_entity_valid_command()
        {
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization{ ConfigureMembers = true });

            
            var command = fixture.Create<AddEntityCommand>();
            var eventBus = fixture.Create<IDomainEventBus>();

            var handler = new AddEntityCommandHandler(eventBus);

            var result = handler.Execute(command);

            eventBus.Received(1).Publish(Arg.Any<EntityAddedEvent>());
            Assert.True(result.IsSucceed);
        }
        
        [Fact]
        public void Add_entity_eventbased_valid_command()
        {
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization{ ConfigureMembers = true });

            
            var command = fixture.Create<AddEntityCommand>();
            var eventBus = fixture.Create<IDomainEventBus>();

            var handler = new AddEntityCommandHandler(eventBus);

            var result = handler.Execute(command);

            eventBus.Received(1).Publish(Arg.Any<EntityAddedEvent>());
            Assert.True(result.IsSucceed);
        }
    }
}
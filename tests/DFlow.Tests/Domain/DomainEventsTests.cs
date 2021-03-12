// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using DFlow.Domain.DomainEvents;
using DFlow.Domain.EventBus.FluentMediator;
using DFlow.Domain.Events;
using DFlow.Tests.Supporting.DomainObjects;
using FluentMediator;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Xunit;

namespace DFlow.Tests.Domain
{
    public sealed class DomainEventsTests
    {
        [Fact]
        public void DomainEvent_Publishing()
        {
            var fixture = new Fixture()
                .Customize(new AutoNSubstituteCustomization(){ ConfigureMembers = true });

            var realEventBus = fixture.Create<IMediator>();
            var myEventBus = new FluentMediatorDomainEventBus(realEventBus);
            var myEvent = fixture.Create<IDomainEvent>();
            myEventBus.Publish(myEvent);
            
            realEventBus.Received().Publish(Arg.Any<IDomainEvent>());
        }
    }
}
// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Threading;
using System.Threading.Tasks;
using DFlow.Domain.Events;
using DFlow.Samples.Domain.Aggregates.Events;

namespace DFlow.Samples.Business.DomainEventHandlers
{
    public sealed class UserAddedDomainEventHandler : DomainEventHandler<UserAddedEvent>
    {
        protected override Task ExecuteHandle(UserAddedEvent @event, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[{nameof(UserAddedEvent)}] event: {@event.Id}: name: {@event.Name} date: {@event.When}");
            return Task.CompletedTask;
        }
    }
}
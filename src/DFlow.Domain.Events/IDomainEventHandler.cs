// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace DFlow.Domain.Events
{
    public interface IDomainEventHandler<in TDomainEvent> where TDomainEvent:IDomainEvent
    {
        void Handle(TDomainEvent @event);
    }
}
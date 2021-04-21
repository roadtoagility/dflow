// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;

namespace DFlow.Domain.Events
{
    public abstract class DomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent> where TDomainEvent:IDomainEvent
    {
        protected Exception Exception { get; set; }

        public void Handle(TDomainEvent @event)
        {
            try
            {
                ExecuteHandle(@event);
            }
            catch (Exception ex)
            {
                //TODO: log here
                Exception = ex;
                throw;
            }
        }

        protected abstract void ExecuteHandle(TDomainEvent @event);
    }
}
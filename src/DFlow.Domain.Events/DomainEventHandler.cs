// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace DFlow.Domain.Events
{
    public abstract class DomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>,
        IDomainEventHandlerAsync<TDomainEvent>
        
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

        public Task HandleAsync(TDomainEvent @event, CancellationToken cancellationToken)
        {
            try
            {
                return ExecuteHandleAsync(@event,cancellationToken);
            }
            catch (Exception ex)
            {
                //TODO: log here
                Exception = ex;
                throw;
            }
        }

        protected virtual void ExecuteHandle(TDomainEvent @event)
        {
            throw new NotImplementedException();
        }

        protected virtual Task ExecuteHandleAsync(TDomainEvent @event, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
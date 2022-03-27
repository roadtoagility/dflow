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
    public abstract class DomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
    {
        protected Exception Exception { get; set; }

        public async Task Handle(TDomainEvent @event)
        {
            var cancellationToken = new CancellationTokenSource();
            await Handle(@event, cancellationToken.Token)
                .ConfigureAwait(false);
        }

        public async Task Handle(TDomainEvent @event, CancellationToken cancellationToken)
        {
            try
            {
                await ExecuteHandle(@event, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //TODO: log here
                Exception = ex;
                throw;
            }
        }

        protected abstract Task ExecuteHandle(TDomainEvent @event, CancellationToken cancellationToken);
    }
}
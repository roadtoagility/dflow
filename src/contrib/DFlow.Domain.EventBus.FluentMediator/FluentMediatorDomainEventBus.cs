// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Threading;
using System.Threading.Tasks;
using DFlow.Domain.Events;
using FluentMediator;

namespace DFlow.Domain.EventBus.FluentMediator
{
    public class FluentMediatorDomainEventBus:IDomainEventBus
    {
        private readonly IMediator _mediator;
        
        public FluentMediatorDomainEventBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Publish<TEvent>(TEvent request)
        {
            var cancellationToken = new CancellationTokenSource();
            await Publish<TEvent>(request, cancellationToken.Token);
        }
        
        public async Task Publish<TEvent>(TEvent request, CancellationToken cancellationToken)
        {
            await _mediator.PublishAsync(request, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<TResult> Send<TResult, TRequest>(TRequest request)
        {
            var cancellationToken = new CancellationTokenSource();
            return await Send<TResult, TRequest>(request,cancellationToken.Token);
        }
        
        public async Task<TResult> Send<TResult,TRequest>(TRequest request, CancellationToken cancellationToken)
        {
            return await _mediator.SendAsync<TResult>(request, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
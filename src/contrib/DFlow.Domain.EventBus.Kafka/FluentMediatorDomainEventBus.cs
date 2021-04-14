// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

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

        public void Publish<TEvent>(TEvent request)
        {
            _mediator.Publish(request);
        }

        public TResult Send<TResult,TRequest>(TRequest request)
        {
            return _mediator.Send<TResult>(request);
        }
    }
}
using System;
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
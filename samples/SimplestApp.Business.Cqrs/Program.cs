// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using DFlow.Business.Cqrs.CommandHandlers;
using DFlow.Domain.EventBus.FluentMediator;
using DFlow.Domain.Events;
using DFlow.Samples.Business.CommandHandlers;
using DFlow.Samples.Business.DomainEventHandlers;
using DFlow.Samples.Domain.Aggregates.Events;
using FluentMediator;
using Microsoft.Extensions.DependencyInjection;

namespace SimplestApp.Business.Cqrs
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("== Simple Cqrs App to Create a User");

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddFluentMediator(builder =>
            {
                builder.On<AddUserCommand>().PipelineAsync()
                    .Return<CommandResult<Guid>, AddUserCommandHandler>(
                        async (handler, request) => await handler.Execute(request));

                builder.On<UserAddedEvent>()
                    .Pipeline()
                    .Call<IDomainEventHandler<UserAddedEvent>>(
                        (handler, request) => handler.Handle(request));
            });

            serviceCollection.AddScoped<IDomainEventBus, FluentMediatorDomainEventBus>();
            serviceCollection.AddScoped<AddUserCommandHandler>();
            serviceCollection.AddScoped<IDomainEventHandler<UserAddedEvent>, UserAddedDomainEventHandler>();

            var provider = serviceCollection.BuildServiceProvider();
            var mediator = provider.GetService<IMediator>();


            var result = mediator?.SendAsync<CommandResult<Guid>>(new AddUserCommand("my name", "mail@test.com"))
                .GetAwaiter()
                .GetResult();

            Console.WriteLine();
            Console.WriteLine($"Add user request id {result.Id} operation succed: {result.IsSucceed}");

            Console.WriteLine("press any key to exit.");
            Console.ReadKey();
        }
    }
}
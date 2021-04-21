﻿// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using DFlow.Business.Cqrs.CommandHandlers;
using DFlow.Domain.EventBus.FluentMediator;
using DFlow.Domain.Events;
using DFlow.Persistence;
using DFlow.Persistence.EntityFramework;
using DFlow.Samples.Business.CommandHandlers;
using DFlow.Samples.Business.DomainEventHandlers;
using DFlow.Samples.Domain.Aggregates.Events;
using DFlow.Samples.Persistence;
using DFlow.Samples.Persistence.Model.Repositories;
using FluentMediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SimplestApp.Business.Cqrs
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("== Simple Cqrs App to Create a User");
            
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddFluentMediator(builder =>
            {
                builder.On<AddUserCommand>().Pipeline()
                    .Return<CommandResult<Guid>, AddUserCommandHandler>(
                        (handler, request) => handler.Execute(request));
                
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

            var result = mediator?.Send<CommandResult<Guid>>(new AddUserCommand("my name", "mail@test.com"));
            
            Console.WriteLine();
            Console.WriteLine($"Add user request id {result.Id} operation succed: {result.IsSucceed}");
            
            Console.WriteLine("press any key to exit.");
            Console.ReadKey();
        }
    }
}
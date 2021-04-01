// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using DFlow.Business.Cqrs.CommandHandlers;
using DFlow.Domain.EventBus.FluentMediator;
using DFlow.Domain.Events;
using DFlow.Persistence;
using DFlow.Samples.Business.CommandHandlers;
using DFlow.Samples.Business.DomainEventHandlers;
using DFlow.Samples.Business.QueryHandlers;
using DFlow.Samples.Domain.Aggregates.Events;
using DFlow.Samples.Persistence;
using DFlow.Samples.Persistence.Model.Repositories;
using DFlow.Samples.Persistence.ReadModel.Repositories;
using FluentMediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SimplestApp.Persistence.EntityFramework
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
                    .Return<CommandResult<Guid>, AddUserPersistentCommandHandler>(
                        (handler, request) => handler.Execute(request));
                
                builder.On<UserAddedEvent>()
                    .Pipeline()
                    .Call<IDomainEventHandler<UserAddedEvent>>(
                        (handler, request) => handler.Handle(request));
                
                builder.On<GetUsersByFilter>().Pipeline()
                    .Return<GetUsersResponse, GetUsersByQueryHandler>(
                        (handler, request) => handler.Execute(request));
            });
            serviceCollection.AddDbContext<SampleAppDbContext>(options =>
                options.UseSqlite("Data Source=samplesdb_dev.sqlite;"));

            serviceCollection.AddSingleton(new SampleAppProjectionDbContext("Filename=sample_projection.db;Connection=shared"));
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IUserProjectionRepository, UserProjectionRepository>();
            serviceCollection.AddScoped<IDbSession<IUserRepository>, SampleDbSession<IUserRepository>>();
            serviceCollection.AddScoped<IDbSession<IUserProjectionRepository>, SampleProjectionDbSession<IUserProjectionRepository>>();
            serviceCollection.AddScoped<IDomainEventBus, FluentMediatorDomainEventBus>();
            serviceCollection.AddScoped<AddUserPersistentCommandHandler>();
            serviceCollection.AddScoped<GetUsersByQueryHandler>();
            serviceCollection.AddScoped<IDomainEventHandler<UserAddedEvent>, UserAddedUpdateProjectionDomainEventHandler>();

            var provider = serviceCollection.BuildServiceProvider();
            var mediator = provider.GetService<IMediator>();

            var result = mediator?.Send<CommandResult<Guid>>(new AddUserCommand("my name", "mail@test.com"));
                
            Console.WriteLine();
            Console.WriteLine($"Add user request id {result.Id} operation succed: {result.IsSucceed}");

            var query = mediator?.Send<GetUsersResponse>(GetUsersByFilter.From("my name"));
            Console.WriteLine($"Add user request id {query.Data.Count} operation succed: {query.Data[0].Name}");
            
            Console.WriteLine("press any key to exit.");
            Console.ReadKey();
        }
    }
}
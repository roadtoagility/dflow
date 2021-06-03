// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using DFlow.Domain.Events;
using Microsoft.Extensions.Configuration;

namespace DFlow.Domain.EventBus.Kafka
{
    public sealed class UserAddedUpdateProjectionDomainEventHandler<TDomainEvent> : DomainEventHandler<TDomainEvent>
    {
        private readonly IConsumer<Null, byte[]> _consumer;

        /// <summary>
        // "Kafka": {
        //     "ProducerSettings": {
        //         "BootstrapServers": "localhost:9092"
        //     },
        //     "ConsumerSettings": {
        //         "BootstrapServers": "localhost:9092",
        //         "GroupId": "web-example-group"
        //     },
        //     "RequestTimeTopic": "request_times",
        //     "FrivolousTopic": "frivolous_topic"
        // } 
        /// </summary>
        /// <param name="sectionConfig"></param>
        public UserAddedUpdateProjectionDomainEventHandler(IConfiguration sectionConfig)
        {
            var config = new ConsumerConfig()
            {
                ClientId = "",
                BootstrapServers = "",
                Acks = Acks.Leader,
                // EnableIdempotence = true
            };
            //sectionConfig.GetSection("Kafka:ProducerSettings").Bind(config);
            // {
            //     Acks = Acks.Leader,
            //     BootstrapServers = "",
            //     EnableIdempotence = true
            // };
            
            _consumer = new ConsumerBuilder<Null, byte[]>(config)
                .Build();
        }

        protected override async Task ExecuteHandle(TDomainEvent @event, CancellationToken cancellationToken)
        {

            while (!cancellationToken.IsCancellationRequested)
            {
                var msg = _consumer.Consume(cancellationToken);

            }
            
            return Task.CompletedTask;
        }
    }
}
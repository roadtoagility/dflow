// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using DFlow.Domain.Events;
using Microsoft.Extensions.Configuration;

namespace DFlow.Domain.EventBus.Kafka
{
    public class ProducerDomainEventBus:IDomainEventBus
    {
        private readonly IProducer<Null, byte[]> _producer;
        
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
        public ProducerDomainEventBus(IConfiguration sectionConfig)
        {
            var config = new ProducerConfig();
            //sectionConfig.GetSection("Kafka:ProducerSettings").Bind(config);
            // {
            //     Acks = Acks.Leader,
            //     BootstrapServers = "",
            //     EnableIdempotence = true
            // };
            
            _producer = new ProducerBuilder<Null, byte[]>(config)
                .Build();
        }

        public async Task Publish<TEvent>(TEvent request)
        {
            var cancellation = new CancellationToken();
            await Publish(request, cancellation).ConfigureAwait(false);
        }

        public async Task Publish<TEvent>(TEvent request, CancellationToken cancellationToken)
        {
            var topic = request.GetType().FullName;
            var msg = request as IDomainEvent; 
            var message = new Message<Null, byte[]>() {Value = SerializeData(msg)};
            await _producer.ProduceAsync(topic, message, cancellationToken);
        }

        public async Task<TResult> Send<TResult,TRequest>(TRequest request)
        {
            var cancellation = new CancellationToken();
            return await Send<TResult, TRequest>(request, cancellation)
                .ConfigureAwait(false);
        }

        public Task<TResult> Send<TResult, TRequest>(TRequest request, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
        
        public static byte[] SerializeData(IDomainEvent evt)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using var mem = new MemoryStream();
            formatter.Serialize(mem, evt);
            return mem.ToArray();
        }
    }
}
// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Ecommerce.Domain.Events.Exported;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql.Replication;
using OperationCanceledException = System.OperationCanceledException;

namespace Ecommerce.Persistence.InboxPublishing;

// https://debezium.io/blog/2019/02/19/reliable-microservices-data-exchange-with-the-outbox-pattern/
// https://pkritiotis.io/outbox-pattern-implementation-challenges/
// https://medium.com/design-microservices-architecture-with-patterns/outbox-pattern-for-microservices-architectures-1b8648dfaa27
// https://medium.com/engineering-varo/event-driven-architecture-and-the-outbox-pattern-569e6fba7216
// https://thorben-janssen.com/outbox-pattern-hibernate/

// how topic naming and schema registry works
//https://docs.confluent.io/platform/current/schema-registry/serdes-develop/index.html#how-the-naming-strategies-work

// schema registry tutorial 
// https://docs.confluent.io/platform/current/schema-registry/schema_registry_onprem_tutorial.html#using-curl-to-interact-with-schema-registry


public class ProductInboxWorker : BackgroundService
{
    private readonly ILogger _logger;
    private readonly string _brokerEndpoints;
    private ConsumerConfig _consumerConfig;

    public ProductInboxWorker(ILogger<ProductInboxWorker> logger, IConfiguration configuration)
    {
        this._brokerEndpoints = configuration.GetSection("MessagingEndpoints:Brokers")!.Value;
        this._logger = logger;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        this._consumerConfig = new ConsumerConfig()
        {
            BootstrapServers = this._brokerEndpoints,
            GroupId = "ecommerce-aggregate-product-consumer-group"
        };

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {

            var topicName = "ecommerce.product";

            using var consumer = new ConsumerBuilder<string, ProductAggregate>(this._consumerConfig)
                .SetValueDeserializer(new ProtobufDeserializer<ProductAggregate>().AsSyncOverAsync())
                .Build();
            consumer.Subscribe(topicName);
            
            try
            {
                while (true)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(cancellationToken);
                        var headerEventIdKey = consumeResult.Message.Headers[0].Key;
                        var headerEventIdValue = consumeResult.Message.Headers[0].GetValueBytes();

                        this._logger.LogInformation(
                            message:
                            $"key: {consumeResult.Message.Key}: {headerEventIdKey}, event id: {headerEventIdValue}");
                        
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Consume error: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                consumer.Close();
                this._logger.LogError("Publishing failed: {ex}", ex);
            }

            await Task.Delay(100, cancellationToken);
        }
    }
}

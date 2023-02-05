// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Nodes;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Ecommerce.Domain.Events.Exported;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Npgsql.Replication;
using Npgsql.Replication.PgOutput;
using Npgsql.Replication.PgOutput.Messages;
using Timestamp = Google.Protobuf.WellKnownTypes.Timestamp;

namespace Ecommerce.Persistence.OutboxPublishing;

// https://debezium.io/blog/2019/02/19/reliable-microservices-data-exchange-with-the-outbox-pattern/
// https://pkritiotis.io/outbox-pattern-implementation-challenges/
// https://medium.com/design-microservices-architecture-with-patterns/outbox-pattern-for-microservices-architectures-1b8648dfaa27
// https://medium.com/engineering-varo/event-driven-architecture-and-the-outbox-pattern-569e6fba7216
// https://thorben-janssen.com/outbox-pattern-hibernate/

// how topic naming and schema registry works
//https://docs.confluent.io/platform/current/schema-registry/serdes-develop/index.html#how-the-naming-strategies-work

// schema registry tutorial 
// https://docs.confluent.io/platform/current/schema-registry/schema_registry_onprem_tutorial.html#using-curl-to-interact-with-schema-registry

// https://debezium.io/blog/2020/02/10/event-sourcing-vs-cdc/

public class ProductOutboxWorker : OutboxWorkerBase
{
    private readonly ILogger _logger;
    private readonly string _schemaRegistryEndpoints;
    private readonly string _brokerEndpoints;
    private SchemaRegistryConfig _schemaRegistryConfig = null!;
    private ProducerConfig _producerConfig = null!;
    public ProductOutboxWorker(ILogger<ProductOutboxWorker> logger, IConfiguration configuration)
    :base(logger, configuration)
    {
        this._schemaRegistryEndpoints = configuration.GetSection("MessagingEndpoints:SchemaRegistry")!.Value;
        this._brokerEndpoints = configuration.GetSection("MessagingEndpoints:Brokers")!.Value;
        this._logger = logger;
    }
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        this._producerConfig = new ProducerConfig()
        {
            BootstrapServers = this._brokerEndpoints
        };
        
        this._schemaRegistryConfig = new SchemaRegistryConfig
        {
            Url = this._schemaRegistryEndpoints
        };

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await using var dbConnection = new LogicalReplicationConnection(this._connectionString);
            await dbConnection.Open(cancellationToken);

            var slot = new PgOutputReplicationSlot("products_outbox_slot");
            var outputOptions = new PgOutputReplicationOptions("products_outbox_pub", 2);
            await foreach (var message in dbConnection.StartReplication(slot, outputOptions, cancellationToken))
            {
                var indColumn = 0;
                if (message is not InsertMessage insert)
                {
                    continue;
                }

                var eventTimeProcessingMs = new DateTimeOffset(insert.ServerClock);
                var cols = new Dictionary<string, string>();
                var columns = insert.NewRow.GetAsyncEnumerator(cancellationToken);
                    
                while (await columns.MoveNextAsync())
                {
                    cols[insert.Relation.Columns[indColumn++].ColumnName] = 
                        await columns.Current.Get<string>(cancellationToken);
                }
                    
                var topicName = $@"{insert.Relation.Namespace}.{cols["aggregation_type"].ToLower()}";
                var headerId = new Header("eventId", Guid.Parse(cols["id"]).ToByteArray());
                var topicAggregationKey = cols["aggregate_id"];
                var payloadJson = JsonSerializer.Deserialize<JsonNode>(cols["event_data"]);
                
                var body = new ProductAggregate
                {
                    EventType = cols["event_type"],
                    EventProcessingTimeMs = eventTimeProcessingMs.ToUnixTimeMilliseconds(),
                };

                switch (cols["event_type"])
                {
                    case nameof(ProductCreatedEvent):
                        body.ProductCreated = new ProductCreatedEvent
                        {
                            Name = payloadJson!["Name"]!.GetValue<string>(),
                            Description = payloadJson!["Description"]!.GetValue<string>(),
                            Weight = payloadJson!["Weight"]!.GetValue<double>(),
                            EventTime = Timestamp.FromDateTimeOffset(
                                DateTimeOffset.Parse(payloadJson!["When"]!.GetValue<string>()))
                        };
                    break;
                    case nameof(ProductUpdatedEvent):
                        body.ProductUpdated = new ProductUpdatedEvent
                        {
                            Id = payloadJson!["Id"]!.GetValue<string>(),
                            Description = payloadJson!["Description"]!.GetValue<string>(),
                            Weight = payloadJson!["Weight"]!.GetValue<double>(),
                            EventTime = Timestamp.FromDateTimeOffset(
                                DateTimeOffset.Parse(payloadJson!["When"]!.GetValue<string>()))
                        };
                    break;
                }
                
                using var schemaRegistry = new CachedSchemaRegistryClient(this._schemaRegistryConfig);
                using var producer = new ProducerBuilder<string, ProductAggregate>(this._producerConfig)
                    .SetValueSerializer(new ProtobufSerializer<ProductAggregate>(schemaRegistry))
                    .Build();
                try
                {
                    var outboxMessage = new Message<string, ProductAggregate>
                    {
                        Key = topicAggregationKey, 
                        Value = body ,
                        Headers = new Headers {headerId}
                    };
                    var dr = await producer
                        .ProduceAsync(topicName, outboxMessage,cancellationToken);
                        
                    this._logger.LogInformation(message: "New topic offset from published message: {0}"
                        , dr.TopicPartitionOffset);
                        
                    // Always call SetReplicationStatus() or assign LastAppliedLsn and LastFlushedLsn individually
                    // so that Npgsql can inform the server which WAL files can be removed/recycled.
                    dbConnection.SetReplicationStatus(insert.WalEnd);
                }
                catch (ProduceException<Guid, ProductAggregate> ex)
                {
                    // In some cases (notably Schema Registry connectivity issues), the InnerException
                    // of the ProduceException contains additional information pertaining to the root
                    // cause of the problem. This information is automatically included in the output
                    // of the ToString() method of the ProduceException, called implicitly in the below.
                    this._logger.LogError("Publishing failed: {ex}", ex);
                }
            }

            await Task.Delay(100, cancellationToken);
        }
    }
}

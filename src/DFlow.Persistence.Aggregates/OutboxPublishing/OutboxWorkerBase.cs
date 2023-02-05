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

public abstract class OutboxWorkerBase : BackgroundService
{
    private readonly ILogger _logger;
    protected readonly string _connectionString;

    protected OutboxWorkerBase(ILogger logger, IConfiguration configuration)
    {
        this._connectionString = configuration.GetConnectionString("ModelConnection")!;
        this._logger = logger;
    }

    private async Task PrepareLogicalReplicationAsync(CancellationToken cancellationToken)
    {
        var conn = new NpgsqlConnection(this._connectionString);
        await conn.OpenAsync(cancellationToken);

        await using var checkPublication =
            new NpgsqlCommand("select exists(select 1 from pg_publication where pubname = @pubName)", conn);
        checkPublication.Parameters.Add(new NpgsqlParameter<string>("pubName", "products_outbox_pub"));
        await checkPublication.PrepareAsync(cancellationToken);
        var hasPublication = (bool) (await checkPublication.ExecuteScalarAsync(cancellationToken))!;

        if (hasPublication == false)
        {
            string insertPub = "CREATE PUBLICATION products_outbox_pub FOR TABLE products_outbox WITH (publish = 'insert')"; 
            await using var insertPublication = new NpgsqlCommand(insertPub, conn);
            await insertPublication.PrepareAsync(cancellationToken);
            await insertPublication.ExecuteNonQueryAsync(cancellationToken);
        }
        
        await using var checkSlotReplication = new NpgsqlCommand("select exists(select 1 from pg_replication_slots where slot_name = @slotName)",conn);
        checkSlotReplication.Parameters.Add(new NpgsqlParameter<string>("slotName", "products_outbox_slot"));
        await checkSlotReplication.PrepareAsync(cancellationToken);
        var hasSlot = (bool) (await checkSlotReplication.ExecuteScalarAsync(cancellationToken))!;

        if (hasSlot == false)
        {
            await using var createSlot =
                new NpgsqlCommand("SELECT * FROM pg_create_logical_replication_slot( @slotName, 'pgoutput')", conn);
            createSlot.Parameters.Add(new NpgsqlParameter<string>("slotName", "products_outbox_slot"));
            await createSlot.PrepareAsync(cancellationToken);
            await createSlot.ExecuteNonQueryAsync(cancellationToken);            
        }
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await base.StartAsync(cancellationToken);
    }
}

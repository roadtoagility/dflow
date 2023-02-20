// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DFlow.Persistence.EntityFrameworkCore.Mappings;

public class AggregateStateMapping:IEntityTypeConfiguration<AggregateState>
{
    public void Configure(EntityTypeBuilder<AggregateState> builder)
    {
        builder.ToTable("transaction_log_outbox");
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedNever()
            .IsRequired();
        builder.HasKey(e => e.Id);

        builder.Property(e => e.AggregateId)
            .HasColumnName("aggregate_id");
        builder.Property(e => e.AggregationType)
            .HasColumnName("aggregation_type");
        builder.Property(e => e.EventType)
            .HasColumnName("event_type");
        builder.Property(e => e.EventData)
            .HasColumnName("event_data")
            .HasColumnType("jsonb");
        builder.Property(e => e.EventDatetime)
            .HasColumnName("event_time")
            .HasColumnType("timestamp with time zone");
    }
}
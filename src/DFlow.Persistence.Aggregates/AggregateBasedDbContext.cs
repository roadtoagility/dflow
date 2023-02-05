// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Ecommerce.Persistence;
using Ecommerce.Persistence.Mappings;
using Ecommerce.Persistence.State;
using Microsoft.EntityFrameworkCore;

namespace DFlow.Persistence.Aggregates;

public sealed class AggregateBasedDbContext : SoftDeleteDbContext
{
    public AggregateBasedDbContext(DbContextOptions<AggregateBasedDbContext> contextOptions)
        : base(contextOptions)
    {

    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new AggregateStateMapping().Configure(modelBuilder.Entity<AggregateState>());
    }
}
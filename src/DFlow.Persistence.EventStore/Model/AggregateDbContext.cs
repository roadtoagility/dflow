// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using Microsoft.EntityFrameworkCore;

namespace DFlow.Persistence.EventStore.Model
{
    public class AggregateDbContext : DbContext
    {
        public AggregateDbContext(DbContextOptions options)
            : base(options)
        {
        }
        
        public DbSet<EventData<Guid>> Aggregates { get; set; }

        public override int SaveChanges()
        {
            UpdateSoftDeleteLogic();
            return base.SaveChanges();
        }

        private void UpdateSoftDeleteLogic()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.CurrentValues["IsDeleted"] = true;
                }
                else
                {
                    entry.CurrentValues["IsDeleted"] = false;
                }
            }
        }
    }
}
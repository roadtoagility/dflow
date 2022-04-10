// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.Persistence.EntityFramework.Model;
using DFlow.Samples.Persistence.Model;
using DFlow.Samples.Persistence.ReadModel;
using Microsoft.EntityFrameworkCore;

namespace DFlow.Samples.Persistence
{
    public class SampleAppDbContext : SoftDeleteDbContext
    {
        public SampleAppDbContext(DbContextOptions<SampleAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserState> Users { get; set; }
        
        public DbSet<UserProjection> UsersProjection { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserState>(
                b =>
                {
                    b.Property(e => e.Id).ValueGeneratedNever().IsRequired();
                    b.Property(e => e.Name).IsRequired();

                    b.Property(p => p.PersistenceId);
                    b.Property(e => e.IsDeleted);
                    b.HasQueryFilter(user => EF.Property<bool>(user, "IsDeleted") == false);
                    b.Property(e => e.CreateAt);
                    b.Property(e => e.RowVersion);
                });

            modelBuilder.Entity<UserProjection>(u =>
            {
                u.Property(pr => pr.Id).ValueGeneratedNever();
                u.HasKey(pr => pr.Id);
                u.Property(pr => pr.Name);
                u.Property(pr => pr.CommercialEmail);
                u.HasQueryFilter(user => EF.Property<bool>(user, "IsDeleted") == false);
            });

        }
    }
}
// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.Persistence.LiteDB.Model;
using DFlow.Samples.Persistence.ReadModel;
using LiteDB;

namespace DFlow.Samples.Persistence
{
    public class SampleAppProjectionDbContext : LiteDbContext
    {
        public SampleAppProjectionDbContext(string connectionString)
            : base(connectionString, BsonMapper.Global)
        {
            Users = Database.GetCollection<UserProjection>("user_projection");

            OnModelCreating();
        }

        public ILiteCollection<UserProjection> Users { get; }

        protected void OnModelCreating()
        {
            ModelBuilder.Entity<UserProjection>()
                .Field(pr => pr.Id, "user_id")
                .Field(pr => pr.Name, "name")
                .Field(pr => pr.CommercialEmail, "email");
        }
    }
}
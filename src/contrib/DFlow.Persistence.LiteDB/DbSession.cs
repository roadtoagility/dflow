// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System;
using System.Threading;
using System.Threading.Tasks;
using DFlow.Persistence.LiteDB.Model;
using LiteDB;

namespace DFlow.Persistence.LiteDB
{
    public class ProjectionDbSession<TRepository>: IDbSession<TRepository>, IDisposable
    {
        public ProjectionDbSession(LiteDbContext context, TRepository repository)
        {
            Context = context;
            Repository = repository;
        }

        private LiteDbContext Context { get; }
        
        public TRepository Repository { get; }

        public void SaveChanges()
        {
            Context.Database.Commit();
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Context.Database.Commit());
        }

        public void Dispose()
        {
            Context?.Database.Dispose();
        }
    }
}
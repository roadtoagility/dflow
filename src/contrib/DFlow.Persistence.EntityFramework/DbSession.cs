// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System;
using Microsoft.EntityFrameworkCore;

namespace DFlow.Persistence.EntityFramework
{
    public class DbSession<TRepository>: IDbSession<TRepository>, IDisposable
    {
        public DbSession(DbContext context, TRepository repository)
        {
            Context = context;
            Repository = repository;
        }

        private DbContext Context { get; }
        
        public TRepository Repository { get; }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
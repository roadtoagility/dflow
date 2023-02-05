// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using Ecommerce.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DFlow.Persistence.Aggregates;

public class DbSession<TRepository> : IDbSession<TRepository>, IDisposable
{
    public DbSession(AggregateBasedDbContext context, TRepository repository)
    {
        Context = context;
        Repository = repository;
        
    }

    private DbContext Context { get; }

    public TRepository Repository { get; }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await Context.SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Context.Dispose();
        }
    }
}
// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Collections.Immutable;
using System.Linq.Expressions;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Events.Aggregates;
using DFlow.Persistence.Aggregates;
using DFlow.Persistence.Repositories;
using Ecommerce.Domain;
using Ecommerce.Domain.Aggregates;
using Ecommerce.Persistence.ExtensionMethods;
using Ecommerce.Persistence.State;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Persistence.Repositories;

public class OutboxRepository: IRepository<TAggregate>
{
    private readonly AggregateBasedDbContext _dbContext;
    private readonly int initialPageNumber = 1;
    private readonly int recordPageSizeLimit = 20;

    public ProductRepository(AggregateBasedDbContext dbContext)
    {
        this._dbContext = dbContext;
    }
    public async Task Add(Product entity, CancellationToken cancellationToken = default)
    {
       
        var outbox = entity.ToOutBox();
        await this._dbContext
            .Set<AggregateState>()
            .AddRangeAsync(outbox,cancellationToken);
    }

    public async Task Remove(Product entity, CancellationToken cancellationToken = default)
    {
        var outbox = entity.ToOutBox();
        await this._dbContext
            .Set<AggregateState>()
            .AddRangeAsync(outbox,cancellationToken);
    }

    public async Task<IReadOnlyList<Product>> FindAsync(Expression<Func<ProductState, bool>> predicate
        , CancellationToken cancellationToken)
    {
        return await FindAsync(predicate, this.initialPageNumber, this.recordPageSizeLimit, cancellationToken);
    }

    public async Task<Product> GetById(ProductId id, CancellationToken cancellation)
    {
        var result = await FindAsync(p => p.Id.Equals(id.Value), cancellation);

        return result.Count == 0 ? Product.Empty() : result.First();
    }

    public async Task<IReadOnlyList<Product>> FindAsync(Expression<Func<ProductState, bool>> predicate,
        int pageNumber,
        int pageSize, CancellationToken cancellationToken)
    {
        try
        {
            return await this._dbContext.Set<ProductState>()
                .Where(predicate).AsNoTracking()
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .Select(t => t.ToProduct())
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        catch (InvalidOperationException)
        {
            return ImmutableList<Product>.Empty;
        }
    }
}
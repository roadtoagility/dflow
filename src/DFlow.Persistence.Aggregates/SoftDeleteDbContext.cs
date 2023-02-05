// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Persistence;

public class SoftDeleteDbContext : DbContext
{
    public SoftDeleteDbContext(DbContextOptions options)
        : base(options)
    {
    }

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
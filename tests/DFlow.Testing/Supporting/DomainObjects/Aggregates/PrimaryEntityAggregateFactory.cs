// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System;
using DFlow.Domain.Aggregates;

namespace DFlow.Testing.Supporting.DomainObjects.Aggregates;

public sealed class PrimaryEntityAggregateFactory : IAggregateFactory<PrimaryEntityAggregate,PrimaryEntity>
{
    public PrimaryEntityAggregate Create(PrimaryEntity source)
    {
        if (!source.IsValid)
        {
            throw new ArgumentException();
        }
        
        return new PrimaryEntityAggregate(source);
    }
}
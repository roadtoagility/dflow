// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.Domain.Aggregates;
using DFlow.Tests.Supporting.DomainObjects.Commands;

namespace DFlow.Tests.Supporting.DomainObjects
{
    public class ObjectBasedAggregateFactory :
        IAggregateFactory<BusinessEntityAggregateRoot, AddEntityCommand>,
        IAggregateFactory<BusinessEntityAggregateRoot, BusinessEntity>
    {
        public BusinessEntityAggregateRoot Create(AddEntityCommand command)
        {
            return new BusinessEntityAggregateRoot(BusinessEntity.New());
        }

        public BusinessEntityAggregateRoot Create(BusinessEntity source)
        {
            return new BusinessEntityAggregateRoot(source);
        }
    }
}
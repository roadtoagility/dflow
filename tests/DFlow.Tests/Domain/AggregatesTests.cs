// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.Domain.BusinessObjects;
using DFlow.Tests.Supporting.DomainObjects;
using Xunit;

namespace DFlow.Tests.Domain
{
    public sealed class AggregatesTests
    {
        [Fact]
        public void Aggregate_create_a_valid()
        {
            var agg = BusinessEntityAggregateRoot.Create();

            Assert.True(agg.ValidationResults.IsValid);
        }
        
        [Fact]
        public void Aggregate_reconstruct_a_valid()
        {
            var be = BusinessEntity.From(EntityTestId.GetNext(), Version.New());
            var agg = BusinessEntityAggregateRoot.ReconstructFrom(be);

            Assert.True(agg.ValidationResults.IsValid);
        }
    }
}
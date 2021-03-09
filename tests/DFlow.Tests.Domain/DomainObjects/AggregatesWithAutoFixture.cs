// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.Domain.BusinessObjects;
using DFlow.Tests.Domain.DomainObjects.Supporting;
using Xunit;

namespace DFlow.Tests.Domain.DomainObjects
{
    public sealed class AggregatesWithAutoFixture
    {
        [Fact]
        public void Aggregate_create_a_valid()
        {
            var agg = TestAggregateRoot.Create();

            Assert.True(agg.ValidationResults.IsValid);
        }
        
        [Fact]
        public void Aggregate_reconstruct_a_valid()
        {
            var be = BusinessEntity.From(EntityId.GetNext(), Version.New());
            var agg = TestAggregateRoot.ReconstructFrom(be);

            Assert.True(agg.ValidationResults.IsValid);
        }
    }
}
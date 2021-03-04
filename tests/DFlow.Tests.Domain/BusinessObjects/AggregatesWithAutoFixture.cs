// Copyright (C) 2020  Road to Agility
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Library General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Library General Public License for more details.
//
// You should have received a copy of the GNU Library General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 51 Franklin St, Fifth Floor,
// Boston, MA  02110-1301, USA.
//

using System;
using AutoFixture;
using DFlow.Domain.BusinessObjects;
using DFlow.Tests.Domain.BusinessObjects.Supporting;
using Xunit;
using Version = DFlow.Domain.BusinessObjects.Version;

namespace DFlow.Tests.Domain.BusinessObjects
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
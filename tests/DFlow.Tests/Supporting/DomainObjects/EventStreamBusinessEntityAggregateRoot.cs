// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections.Immutable;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Events.Aggregates;
using DFlow.Domain.Events.BusinessObjects;
using DFlow.Tests.Supporting.DomainObjects.Events;

namespace DFlow.Tests.Supporting.DomainObjects
{
    public sealed class EventStreamBusinessEntityAggregateRoot:EventBasedAggregationRoot<EntityTestId>
    {
        internal EventStreamBusinessEntityAggregateRoot(Name name, Email email, VersionId version)
            :base(EntityTestId.GetNext(),version, AggregationName.From(nameof(EventStreamBusinessEntityAggregateRoot)))
        {
            if (name.ValidationStatus.IsValid && email.ValidationStatus.IsValid)
            {
                var change = TestEntityAggregateAddedDomainEvent.From(AggregateId, name, email, version);
                Apply(change);
                
                // it is always new
                Raise(change);
            }
            
            AppendValidationResult(name.ValidationStatus.Failures);
            AppendValidationResult(email.ValidationStatus.Failures);
        }

        internal EventStreamBusinessEntityAggregateRoot(EventStream<EntityTestId> eventStream)
            : base(eventStream.AggregationId, eventStream.Version,
                AggregationName.From(nameof(EventStreamBusinessEntityAggregateRoot)))
        {
            if (eventStream.IsValid)
            {
                Apply(eventStream.Events);
            }
            AppendValidationResult(eventStream.Failures.ToImmutableList());
        }

        public void UpdateName(EntityTestId aggregateId, Name name)
        {
            if (name.ValidationStatus.IsValid && !AggregateId.Equals(aggregateId))
            {
                Apply(TestEntityAggregateUpdatedDomainEvent.From(AggregateId,name,Version));
            }

            AppendValidationResult(name.ValidationStatus.Failures);
        }
    }
}
// Copyright (C) 2021  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace DFlow.Domain.Specifications
{
    public class NotSpecification<TBusinessObject> : CompositeSpecification<TBusinessObject>
    {
        public NotSpecification(ISpecification<TBusinessObject> condition)
        {
            Condition = condition;
        }

        protected ISpecification<TBusinessObject> Condition { get; }

        public override bool IsSatisfiedBy(TBusinessObject candidate)
        {
            return !Condition.IsSatisfiedBy(candidate);
        }
    }
}
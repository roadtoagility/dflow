// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.Domain.Specifications;

namespace DFlow.Specifications
{
    public abstract class CompositeSpecification<TBusinessObject> : ISpecification<TBusinessObject>
    {
        public ISpecification<TBusinessObject> And(ISpecification<TBusinessObject> candidate)
        {
            return new AndSpecification<TBusinessObject>(this, candidate);
        }

        public ISpecification<TBusinessObject> Or(ISpecification<TBusinessObject> candidate)
        {
            return new OrSpecification<TBusinessObject>(this, candidate);
        }

        public ISpecification<TBusinessObject> Not()
        {
            return new NotSpecification<TBusinessObject>(this);
        }

        public abstract bool IsSatisfiedBy(TBusinessObject candidate);
    }
}
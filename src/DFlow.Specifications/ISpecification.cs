// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace DFlow.Specifications
{
    public interface ISpecification<TBusinessObject>
    {
        bool IsSatisfiedBy(TBusinessObject candidate);
        ISpecification<TBusinessObject> And(ISpecification<TBusinessObject> other);
        ISpecification<TBusinessObject> Or(ISpecification<TBusinessObject> other);
        ISpecification<TBusinessObject> Not();
    }
}
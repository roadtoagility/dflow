// Copyright (C) 2021  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Linq.Expressions;

namespace DFlow.Domain.Specifications
{
    public abstract class LinqExpressionSpecification<TBusinessObject>
        :CompositeSpecification<TBusinessObject>
    {
        protected abstract Expression<Func<TBusinessObject, bool>> AsExpression();
        
        public override bool IsSatisfiedBy(TBusinessObject candidate) 
            => AsExpression().Compile()(candidate);
    }
}

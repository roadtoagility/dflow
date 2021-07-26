// Copyright (c) 2021 Harry McIntyre
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentValidation;
using FluentValidation.Results;

namespace DFlow.Domain.BusinessObjects
{
    public class ValueOf<TValue, TThis, TValidator> 
        where TThis : ValueOf<TValue, TThis, TValidator>
        where TValidator: AbstractValidator<TThis>, new()
    {
        private static readonly Func<TValue,TThis> Factory;

        public ValidationResult ValidationStatus { get; private set; }

        static ValueOf()
        {
            ConstructorInfo ctor = typeof(TThis)
                .GetTypeInfo()
                .DeclaredConstructors
                .Where(ctr=> ctr.GetParameters().Length == 1)
                .Select(ctr=> ctr)
                .First();

            var argExpr = Expression.Parameter(typeof(TValue), "value");
            var argsExp = Expression.Add(argExpr,Expression.Constant(1));
            var newExp = Expression.New(ctor, argsExp);
            var lambda = Expression.Lambda(typeof(Func<TValue,TThis>), newExp, 
                new List<ParameterExpression>() { argExpr });

            Factory = (Func<TValue,TThis>)lambda.Compile();
        }

        public TValue Value { get;}

        public static TThis From(TValue item)
        {
            TThis realVo = Factory(item);
            realVo.Validate();

            return realVo;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return obj.GetType() == GetType() && Equals((ValueOf<TValue, TThis, TValidator>)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }

        // Implicit operator removed. See issue #14.

        public override string ToString()
        {
            return Value.ToString();
        }

        protected ValueOf(TValue value)
        {
            Value = value;
        }
        
        protected virtual bool Equals(ValueOf<TValue, TThis,TValidator> other)
        {
            return EqualityComparer<TValue>.Default.Equals(Value, other.Value);
        }

        /// <summary>
        /// WARNING - THIS FEATURE IS EXPERIMENTAL. I may change it to do
        /// validation in a different way.
        /// Right now, override this method, and throw any exceptions you need to.
        /// Access this.Value to check the value
        /// </summary>
        protected virtual void Validate()
        {
            ValidationStatus = new TValidator().Validate((TThis)this);
        }

    }
}
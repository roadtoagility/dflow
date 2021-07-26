// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System;
using AutoFixture;
using DFlow.Domain.BusinessObjects;
using DFlow.Tests.Supporting.DomainObjects;
using FluentValidation;
using Xunit;

namespace DFlow.Tests.Domain
{
    public sealed class ValueOfTests
    {
        [Fact]
        public void ValueOf_create_Vo_isValid()
        {
            var vo = SimpleVo.From("teste");

            Assert.True(vo.ValidationStatus.IsValid);
        }

        [Fact]
        public void ValueOf_create_Vo_not_Valid()
        {
            var vo = SimpleVo.From("");
            Assert.False(vo.ValidationStatus.IsValid);
        }
        
        [Fact]
        public void ValueOf_create_Vo_is_the_same()
        {
            var vo1 = SimpleVo.From("testes");
            var vo2 = SimpleVo.From("testes");

            Assert.True(vo1.Equals(vo2));
        }
        
        [Fact]
        public void ValueOf_create_Vo_is_not_the_same()
        {
            var vo1 = SimpleVo.From("testes 1");
            var vo2 = SimpleVo.From("testes");

            Assert.False(vo1.Equals(vo2));
        }
        
        [Fact]
        public void ValueOf_create_ComplexVo_isValid()
        {
            var vo = ComplexVo.From(("teste", SimpleVo.From("outro vo")));

            Assert.True(vo.ValidationStatus.IsValid);
        }

        [Fact]
        public void ValueOf_create_ComplexVo_not_Valid()
        {
            var vo = ComplexVo.From(("", SimpleVo.From("")));

            Assert.False(vo.ValidationStatus.IsValid);
        }
        
        class ComplexVo : ValueOf<(string,SimpleVo), ComplexVo, ComplexVoValidator>
        {
            private ComplexVo((string,SimpleVo) data)
            :base(data){}
        }
        
        class ComplexVoValidator : AbstractValidator<ComplexVo>
        {
            public ComplexVoValidator()
            {
                RuleFor(x => x.Value.Item1).NotEmpty();
                RuleFor(x => x.Value.Item1).NotNull();
                RuleFor(x => x.Value.Item2).SetValidator(new SimpleVoValidator());
            }   
        }
        class SimpleVo : ValueOf<string, SimpleVo, SimpleVoValidator>
        {
            private SimpleVo(string data)
                :base(data){}
        }
        class SimpleVoValidator : AbstractValidator<SimpleVo>
        {
            public SimpleVoValidator()
            {
                RuleFor(x => x.Value).NotEmpty();
                RuleFor(x => x.Value).NotNull();
            }   
        }
    }
}
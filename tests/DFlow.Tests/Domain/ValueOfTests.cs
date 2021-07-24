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
            var vo = Vo.From("teste");

            Assert.True(vo.ValidationStatus.IsValid);
        }

        [Fact]
        public void ValueOf_create_Vo_not_Valid()
        {
            var vo = Vo.From("");

            Assert.False(vo.ValidationStatus.IsValid);
        }

        [Fact]
        public void ValueOf_create_ComplexVo_isValid()
        {
            var vo = ComplexVo.From(("teste", Vo.From("outro vo")));

            Assert.True(vo.ValidationStatus.IsValid);
        }

        [Fact]
        public void ValueOf_create_ComplexVo_not_Valid()
        {
            var vo = ComplexVo.From(("", Vo.From("")));

            Assert.False(vo.ValidationStatus.IsValid);
        }
        
        class ComplexVo : ValueOf<(string,Vo), ComplexVo, ComplexVoValidator>
        {
        }
        
        class ComplexVoValidator : AbstractValidator<ComplexVo>
        {
            public ComplexVoValidator()
            {
                RuleFor(x => x.Value.Item1).NotEmpty();
                RuleFor(x => x.Value.Item1).NotNull();
                RuleFor(x => x.Value.Item2).SetValidator(new VoValidator());
            }   
        }
        class Vo : ValueOf<string, Vo, VoValidator>
        {
        }
        class VoValidator : AbstractValidator<Vo>
        {
            public VoValidator()
            {
                RuleFor(x => x.Value).NotEmpty();
                RuleFor(x => x.Value).NotNull();
            }   
        }
    }
}
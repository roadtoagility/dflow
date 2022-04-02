// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System;
using DFlow.Domain.Validation;

namespace DFlow.Domain.Command
{
    public class BaseCommand : BaseValidation, ICommand
    {
        public BaseCommand()
        {
            When = DateTime.Now;
        }

        public DateTime When { get; }
    }
}
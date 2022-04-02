// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.Samples.Domain.BusinessObjects;

namespace DFlow.Samples.Business.CommandHandlers
{
    public class AddUserCommand
    {
        public AddUserCommand(string name, string email)
        {
            Name = Name.From(name);
            Mail = Email.From(email);
        }

        public Name Name { get; set; }
        public Email Mail { get; set; }
    }
}
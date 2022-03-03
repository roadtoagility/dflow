// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using DFlow.Samples.Domain.BusinessObjects;
using SimplestApp.Operations;

namespace SimplestApp.Services
{
    public interface IUserService
    {
        User Add(AddUser user);
    }
}
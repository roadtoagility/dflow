// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace DFlow.Business.Cqrs.CommandHandlers
{
    public interface ICommandHandler<in TCommand, out TResult>
    {
        TResult Execute(TCommand command);
    }
}
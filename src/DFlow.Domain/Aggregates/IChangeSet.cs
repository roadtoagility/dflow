// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using DFlow.Domain.Events;

namespace DFlow.Domain.Aggregates
{
    public interface IChangeSet<out TChange>
    {
        TChange GetChange();
        IReadOnlyList<IDomainEvent> GetEvents();
    }
}
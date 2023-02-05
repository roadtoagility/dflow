// Copyright (C) 2023  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Collections.Generic;
using DFlow.BusinessObjects;
using DFlow.Events;

namespace DFlow.Persistence.Outbox;

public interface IEventsExporting<out TChangeSet, in TPrimaryEntity>
{
    IReadOnlyList<TChangeSet> ToOutBox(TPrimaryEntity fromEntity);
}

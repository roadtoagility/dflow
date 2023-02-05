// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System;
using System.Text.Json;
using NodaTime;

namespace DFlow.Persistence.Model;

public sealed record AggregateState(Guid Id
    , Guid AggregateId
    , string AggregationType
    , string EventType
    , Instant EventDatetime
    , JsonDocument EventData):IDisposable
{
    public void Dispose()
    {
        EventData.Dispose();
    }
}
// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Events;

namespace DFlow.Domain.DomainEvents
{
    public class DomainEvent : IDomainEvent
    {
        protected DomainEvent(DateTime when, VersionId version)
        {
            When = when;
            Version = version;
        }

        public VersionId Version { get; }

        public DateTime When { get; }
    }
}
// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Events.DomainEvents;
using DFlow.Samples.Domain.BusinessObjects;

namespace DFlow.Samples.Domain.Aggregates.Events
{
    public class UserAddedEvent : DomainEvent
    {
        private UserAddedEvent(EntityId clientId, Name name, Email mail, VersionId version)
            : base(DateTime.Now, version)
        {
            Id = clientId;
            Name = name;
            Mail = mail;
        }
        public EntityId Id { get; }
        
        public Name Name { get; }
        
        public Email Mail { get; }
        
        public static UserAddedEvent For(User user)
        {
            return new UserAddedEvent(user.Identity,user.Name,user.Mail, user.Version);
        }
    }
}
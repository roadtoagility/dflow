// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System;

namespace DFlow.Persistence.Model
{
    public abstract class PersistentState : IPersistentState
    {
        protected PersistentState(DateTime createAt, byte[] rowVersion)
        {
            PersistenceId = Guid.NewGuid();
            CreateAt = createAt;
            RowVersion = rowVersion;
        }

        public DateTime CreateAt { get; set; }

        public byte[] RowVersion { get; set; }
        public bool IsDeleted { get; set; }

        public Guid PersistenceId { get; set; }
    }
}
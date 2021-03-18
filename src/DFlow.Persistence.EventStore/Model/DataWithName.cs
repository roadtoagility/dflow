// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


namespace DFlow.Persistence.EventStore.Model
{
    public sealed class DataWithName
    {
        public DataWithName(string name, byte[] data)
        {
            Name = name;
            Data = data;
        }

        public string Name { get; private set; }
        public byte[] Data{ get; private set; }
        
    }
}
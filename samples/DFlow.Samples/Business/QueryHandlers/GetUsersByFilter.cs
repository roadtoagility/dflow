// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace DFlow.Samples.Business.QueryHandlers
{
    public class GetUsersByFilter
    {
        private GetUsersByFilter(string name)
        {
            Name = name;
        }

        public string Name { get; }
        
        public static GetUsersByFilter From(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = string.Empty;
            }

            return new GetUsersByFilter(name);
        }
    }
}
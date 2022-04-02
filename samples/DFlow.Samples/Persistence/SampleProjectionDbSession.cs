// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.Persistence.LiteDB;

namespace DFlow.Samples.Persistence
{
    public class SampleProjectionDbSession<TRepository> : ProjectionDbSession<TRepository>
    {
        public SampleProjectionDbSession(SampleAppProjectionDbContext context, TRepository repository)
            : base(context, repository)
        {
        }
    }
}
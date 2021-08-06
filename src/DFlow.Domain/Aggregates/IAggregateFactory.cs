// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.Domain.Command;

namespace DFlow.Domain.Aggregates
{
    public interface IAggregateFactory<out TAggregate, in TCommandCreate, in TReconstruct>
                where TCommandCreate: BaseCommand
    {
        TAggregate Create(TCommandCreate command); 
        
        TAggregate ReconstructFrom(TReconstruct aggregationRoot); 
    }
}
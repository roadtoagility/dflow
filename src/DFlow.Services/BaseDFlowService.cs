// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Threading;
using System.Threading.Tasks;

namespace DFlow.Services
{
    public abstract class BaseDFlowService:IDFlowService
    {
        public string Name { get; private set; }
        public DFlowServiceStatus Status { get; private set; }
        public DFlowServiceRestartingStrategy RestartingStrategy { get; private set; }

        protected BaseDFlowService(string name, DFlowServiceStatus status,
            DFlowServiceRestartingStrategy restartingStrategy)
        {
            Name = name;
            Status = status;
            RestartingStrategy = restartingStrategy;
        }
    }
}
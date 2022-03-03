using System;
using System.Collections.Generic;
using DFlow.Interfaces;

namespace DFlow.Base.Events
{
    public class CommandEvent: IEvent
    {
        public CommandEvent(OperationStatus status, params Exception[] exceptions)
        {
            Status = status;
            Exceptions = exceptions;
        }
        public OperationStatus Status { get; private set; }
        public Exception[] Exceptions { get; private set; }
    }
}
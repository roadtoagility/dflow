using System;
using DFlow.Interfaces;

namespace DFlow.Base.Events
{
    public class CommandEvent : IEvent
    {
        public CommandEvent(OperationStatus status, params Exception[] exceptions)
        {
            Status = status;
            Exceptions = exceptions;
        }

        public OperationStatus Status { get; }
        public Exception[] Exceptions { get; }
    }
}
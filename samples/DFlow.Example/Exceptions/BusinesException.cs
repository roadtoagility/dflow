using System;

namespace DFlow.Example.Exceptions
{
    public class BusinesException : Exception
    {
        public BusinesException(string msg)
            : base(msg)
        {
        }
    }
}
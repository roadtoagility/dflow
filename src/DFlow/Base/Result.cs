using System;
using System.Collections.Generic;

namespace DFlow.Base
{
    public class Result<T>
    {
        public Result(T data, IList<Exception> exceptions)
        {
            Data = data;
            Exceptions = exceptions;
        }

        public T Data { get; }
        public IList<Exception> Exceptions { get; }

        public bool HasExceptions => Exceptions != null && Exceptions.Count > 0;
    }
}
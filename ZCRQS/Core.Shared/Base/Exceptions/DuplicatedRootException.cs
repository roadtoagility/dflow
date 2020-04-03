using System;

namespace Core.Shared.Base.Exceptions
{
    public class DuplicatedRootException : Exception
    {
        public DuplicatedRootException(string key)
            : base($"An aggregate with the same key {key} is already registered")
        {
            
        }
    }
}
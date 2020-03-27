using System;
using System.Collections.Generic;
using Core.Shared;

namespace Program
{
    public class AppendOnlyStore : IAppendOnlyStore
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Append(string name, byte[] data, int expectedVersion = -1)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DataWithVersion> ReadRecords(string name, int afterVersion, int maxCount)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DataWithName> ReadRecords(int afterVersion, int maxCount)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;

namespace Core.Shared
{
    public interface IAppendOnlyStore : IDisposable
    {
        void Append(string name, byte[] data, int expectedVersion = -1);

        IEnumerable<DataWithVersion> ReadRecords(string name, int afterVersion, int maxCount);

        IEnumerable<DataWithName> ReadRecords(int afterVersion, int maxCount);

        void Close();

    }
}
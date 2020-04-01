using System;
using System.Collections.Generic;

namespace Core.Shared
{
    public interface IAppendOnlyStore : IDisposable
    {
        void Append(Guid aggregateId, string aggregateType, byte[] data, int expectedVersion = -1);

        IEnumerable<DataWithVersion> ReadRecords(string name, int afterVersion, int maxCount);
        IEnumerable<DataWithVersion> ReadRecords(Guid aggregateId, int afterVersion, int maxCount);
        IEnumerable<DataWithVersion> ReadRecords<T>(int afterVersion, int maxCount);

        IEnumerable<DataWithName> ReadRecords(int afterVersion, int maxCount);

        void Close();

    }
}
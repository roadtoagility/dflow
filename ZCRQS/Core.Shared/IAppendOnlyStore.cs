using System;
using System.Collections.Generic;

namespace Core.Shared
{
    public interface IAppendOnlyStore<TKey>: IDisposable
    {
        void Append(TKey aggregateId, string aggregateType, byte[] data, long expectedVersion = -1);

        IEnumerable<DataWithVersion> ReadRecords(string name, long afterVersion, int maxCount);
        IEnumerable<DataWithVersion> ReadRecords(TKey aggregateId, long afterVersion, int maxCount);
        IEnumerable<DataWithVersion> ReadRecords<T>(long afterVersion, int maxCount);

        IEnumerable<DataWithName> ReadRecords(long afterVersion, int maxCount);

        void Close();

    }
}
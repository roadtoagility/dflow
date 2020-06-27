using System;
using System.Collections.Generic;
using Core.Shared.Base;

namespace DFlow.Interfaces
{
    public interface IAppendOnlyStore<TKey>: IDisposable
    {
        void Append(Guid id, string aggregateType, long version, ICollection<IEvent> events);

        IEnumerable<DataWithVersion> ReadRecords(string name, long afterVersion, int maxCount);
        IEnumerable<DataWithVersion> ReadRecords(TKey aggregateId, long afterVersion, int maxCount);
        IEnumerable<DataWithVersion> ReadRecords<T>(long afterVersion, int maxCount);

        IEnumerable<DataWithName> ReadRecords(long afterVersion, int maxCount);
        
        bool Any(TKey aggregateId);

        void Close();

    }
}
using System;
using System.Collections.Generic;

namespace Core.Shared.Interfaces
{
    public interface IReadModel<T>
    {
        T Query(Func<T, bool> query);
        IList<T> QueryAll(Func<T, bool> query = null);
    }
}
using Allocat.DataModel;
using System;
using System.Collections.Generic;
using System.Data;

namespace Allocat.DataServiceInterface
{
    public interface IKeyValueDataService : IDataService, IDisposable
    {
        IEnumerable<KeyValue> Get(string type, out TransactionalInformation transaction);
    }
}

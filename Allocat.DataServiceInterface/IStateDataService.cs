using Allocat.DataModel;
using System;
using System.Collections.Generic;
using System.Data;

namespace Allocat.DataServiceInterface
{
    public interface IStateDataService : IDataService, IDisposable
    {
        IEnumerable<State> GetState(out TransactionalInformation transaction);
        State GetStateById(int StateId, out TransactionalInformation transaction);
    }
}

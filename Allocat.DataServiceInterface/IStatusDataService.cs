using Allocat.DataModel;
using System;
using System.Collections.Generic;
using System.Data;

namespace Allocat.DataServiceInterface
{
    public interface IStatusDataService : IDataService, IDisposable
    {
        Status GetStatusByStatusName(string StatusName);
    }
}

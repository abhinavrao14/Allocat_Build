using Allocat.DataModel;
using System;
using System.Collections.Generic;
using System.Data;

namespace Allocat.DataServiceInterface
{
    public interface IHospitalTypeDataService : IDataService, IDisposable
    {
        IEnumerable<HospitalType> GetHospitalType(out TransactionalInformation transaction);
    }
}

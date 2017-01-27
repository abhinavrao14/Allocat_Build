using Allocat.DataModel;
using System;
using System.Collections.Generic;
using System.Data;

namespace Allocat.DataServiceInterface
{
    public interface ICityDataService : IDataService, IDisposable
    {
        IEnumerable<City> GetCitiesByStateId(int StateId,out TransactionalInformation transaction);
        City GetCityById(int CityId, out TransactionalInformation transaction);
    }
}

using Allocat.DataServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using Allocat.DataModel;
using System.Data;
using System.Data.SqlClient;

namespace Allocat.DataService
{
    public class CityDataService : EntityFrameworkDataService, ICityDataService
    {
        public IEnumerable<City> GetCitiesByStateId(int StateId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            dbConnection.Configuration.ProxyCreationEnabled = false;
            IEnumerable<City> lstCity = (from c in dbConnection.City
                                           where c.StateId==StateId
                                           select c).ToList();

            IList<City> listCity = lstCity.ToList();

            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add(listCity.Count.ToString() + " cities found.");

            return listCity;
        }

        public City GetCityById(int CityId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            dbConnection.Configuration.ProxyCreationEnabled = false;
            City city = (from c in dbConnection.City
                           where c.CityID== CityId
                           select c).FirstOrDefault();

            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add("city found.");

            return city;
        }
    }
}

using Allocat.DataServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using Allocat.DataModel;
using System.Data;
using System.Data.SqlClient;

namespace Allocat.DataService
{
    public class HospitalTypeDataService : EntityFrameworkDataService, IHospitalTypeDataService
    {
        public IEnumerable<HospitalType> GetHospitalType(out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            dbConnection.Configuration.ProxyCreationEnabled = false;
            IEnumerable<HospitalType> lstHospitalType = (from ht in dbConnection.HospitalType
                                                  select ht).ToList();

            IList<HospitalType> listHospitalType = lstHospitalType.ToList();

            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add(listHospitalType.Count.ToString()+ " Hospital Types found.");

            return lstHospitalType;
        }
    }
}

using Allocat.DataServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using Allocat.DataModel;
using System.Data;
using System.Data.SqlClient;

namespace Allocat.DataService
{
    public class StatusDataService : EntityFrameworkDataService, IStatusDataService
    {
        public Status GetStatusByStatusName(string StatusName)
        {
            dbConnection.Configuration.ProxyCreationEnabled = false;
            IEnumerable<Status> lstStatus = (from s in dbConnection.Status
                                           where s.StatusName==StatusName
                                           select s).ToList();

            return lstStatus.FirstOrDefault();
        }
    }
}

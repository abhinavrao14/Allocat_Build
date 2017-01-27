using Allocat.DataModel;
using Allocat.DataServiceInterface;
using System;
using System.Collections.Generic;

namespace Allocat.ApplicationService
{
    public class StatusBusinessService
    {
        private IStatusDataService statusDataService;

        public StatusBusinessService(IStatusDataService _statusDataService)
        {
            statusDataService = _statusDataService;
        }

        public Status GetStatusByStatusName(string StatusName)
        {
            Status status = null;
            try
            {
                statusDataService.CreateSession();
                status = statusDataService.GetStatusByStatusName(StatusName);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                statusDataService.CloseSession();
            }

            return status;
        }
    }
}

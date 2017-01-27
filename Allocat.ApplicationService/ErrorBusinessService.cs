using Allocat.DataModel;
using Allocat.DataServiceInterface;
using Allocat.Utility;
using System;
using System.Collections.Generic;

namespace Allocat.ApplicationService
{
    public class ErrorBusinessService
    {
        private IErrorDataService errorDataService;

        public ErrorBusinessService(IErrorDataService _errorDataService)
        {
            errorDataService = _errorDataService;
        }

        public string Error_Create(int StatusId, string ErrorBody, string StackTrace, int? TransactionId, int UserId,string ErrorCode)
        {
            string ret = "";
            try
            {
                errorDataService.CreateSession();
                ret=errorDataService.Error_Create(StatusId, ErrorBody, StackTrace, TransactionId, UserId, ErrorCode);
            }
            catch (Exception ex)
            {
                ret = ex.Message;
            }
            finally
            {
                errorDataService.CloseSession();
            }
            return ret;
        }
    }
}

using Allocat.DataModel;
using Allocat.DataServiceInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Allocat.DataService
{
    public class ErrorDataService : EntityFrameworkDataService, IErrorDataService
    {
        public string Error_Create(int StatusId, string ErrorBody, string StackTrace, int? TransactionId, int UserId,string ErrorCode)
        {
            var parameterStatusId = new SqlParameter("@StatusId", SqlDbType.Int);
            parameterStatusId.Value = StatusId;

            var parameterErrorBody = new SqlParameter("@ErrorBody", SqlDbType.NVarChar);
            parameterErrorBody.Value = ErrorBody;

            var parameterStackTrace = new SqlParameter("@StackTrace", SqlDbType.NVarChar);
            parameterStackTrace.Value = StackTrace;

            var parameterTransactionId = new SqlParameter("@TransactionId", SqlDbType.Int);
            parameterTransactionId.Value = TransactionId;

            var parameterUserId = new SqlParameter("@UserId", SqlDbType.Int);
            parameterUserId.Value = UserId;

            var parameterErrorCode = new SqlParameter("@ErrorCode", SqlDbType.NVarChar);
            parameterErrorCode.Value = ErrorCode;

            int rowAffected = dbConnection.Database.ExecuteSqlCommand("exec dbo.usp_Error_Create @StatusId, @ErrorCode, @ErrorBody, @StackTrace, @TransactionId, @UserId", parameterStatusId, parameterErrorCode,parameterErrorBody, parameterStackTrace, parameterTransactionId, parameterUserId);

            if (rowAffected > 0)
            {
                return "Success";
            }
            else
            {
                return "Failure";
            }
        }
    }
}

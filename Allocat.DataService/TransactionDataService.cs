using Allocat.DataModel;
using Allocat.DataServiceInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Allocat.DataService
{
    public class TransactionDataService : EntityFrameworkDataService, ITransactionDataService
    {
        public int Transaction_Create(DateTime TransactionInitiateDate, float Amount, int UserId, string RequestBody, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            IEnumerable<Decimal?> lstTransaction = dbConnection.usp_Transaction_Create(TransactionInitiateDate,Convert.ToDecimal(Amount), UserId, RequestBody);

            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add("Transaction Created.");

            var Transaction = lstTransaction.FirstOrDefault();
            int TransactionId= Convert.ToInt32(Transaction.Value);
            return TransactionId;
            //transaction = new TransactionalInformation();
            //int rowAffected = 0;

            //var parameterTransactionInitiateDate = new SqlParameter("@TransactionInitiateDate", SqlDbType.DateTime);
            //parameterTransactionInitiateDate.Value = TransactionInitiateDate;

            //var parameterAmount = new SqlParameter("@Amount", SqlDbType.Decimal);
            //parameterAmount.Value = Amount;

            //var parameterUserId = new SqlParameter("@UserId", SqlDbType.Int);
            //parameterUserId.Value = UserId;

            //var parameterRequestBody = new SqlParameter("@RequestBody", SqlDbType.VarChar);
            //parameterRequestBody.Value = RequestBody;

            ////var TransactionId = new SqlParameter();
            ////TransactionId.ParameterName = "@TransactionId";
            ////TransactionId.SqlDbType = SqlDbType.Int;
            ////TransactionId.Direction = ParameterDirection.Output;

            ////var rr = new SqlParameter();
            ////TransactionId.ParameterName = "@rr";
            ////TransactionId.SqlDbType = SqlDbType.Int;
            ////TransactionId.Direction = ParameterDirection.Output;
            //var p = new SqlParameter
            //{
            //    ParameterName = "TransactionId",
            //    DbType = System.Data.DbType.Int32,
            //    Size = 100,
            //    Direction = System.Data.ParameterDirection.Output
            //};

            //var ret = dbConnection.Database.SqlQuery<string>("exec dbo.usp_Transaction_Create @TransactionInitiateDate, @Amount, @UserId, @RequestBody,@TransactionId OUT", parameterTransactionInitiateDate, parameterAmount, parameterUserId, parameterRequestBody, p);

            //var result = ret.First();


            //if (rowAffected > 0)
            //{
            //    transaction.ReturnStatus = true;
            //    transaction.ReturnMessage.Add("Success");
            //}
            //else
            //{
            //    transaction.ReturnStatus = false;
            //    transaction.ReturnMessage.Add("Error");
            //}

            //return rowAffected;
        }

        public int Transaction_Update(int TissueBankId, int TransactionId, string AuthTransactionId, string AuthCode, int StatusId, DateTime TransactionCompleteDate, string ResponseBody, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            int rowAffected = 0;

            var parameterTissueBankId = new SqlParameter("@TissueBankId", SqlDbType.Int);
            parameterTissueBankId.Value = TissueBankId;

            var parameterTransactionId = new SqlParameter("@TransactionId", SqlDbType.Int);
            parameterTransactionId.Value = TransactionId;

            var parameterAuthTransactionId = new SqlParameter("@AuthTransactionId", SqlDbType.NVarChar);
            parameterAuthTransactionId.Value = AuthTransactionId;

            var parameterAuthCode = new SqlParameter("@AuthCode", SqlDbType.VarChar);
            parameterAuthCode.Value = AuthCode;

            var parameterStatusId = new SqlParameter("@StatusId", SqlDbType.Int);
            parameterStatusId.Value = StatusId;

            var parameterTransactionCompleteDate = new SqlParameter("@TransactionCompleteDate", SqlDbType.DateTime);
            parameterTransactionCompleteDate.Value = TransactionCompleteDate;

            var parameterResponseBody = new SqlParameter("@ResponseBody", SqlDbType.VarChar);
            parameterResponseBody.Value = ResponseBody;

           // rowAffected = dbConnection.Database.ExecuteSqlCommand("exec dbo.usp_Transaction_Update @TransactionName, @ContactPersonName, @ContactPersonNumber, @TransactionEmailId, @BusinessURL, @TransactionAddress, @CityId, @TransactionStateLicense, @AATBLicenseNumber, @AATBExpirationDate, @AATBAccredationDate, @UserId, @TransactionId, @TransactionId, @AuthTransactionId, @AuthCode, @StatusId, @TransactionCompleteDate, @ResponseBody", parameterTransactionName, parameterContactPersonName, parameterContactPersonNumber, parameterTransactionEmailId, parameterBusinessURL, parameterTransactionAddress, parameterCityId, parameterTransactionStateLicense, parameterAATBLicenseNumber, parameterAATBExpirationDate, parameterAATBAccredationDate, parameterUserId, parameterTransactionId, parameterTransactionId, parameterAuthTransactionId, parameterAuthCode, parameterStatusId, parameterTransactionCompleteDate, parameterResponseBody);

            if (rowAffected > 0)
            {
                transaction.ReturnStatus = true;
                transaction.ReturnMessage.Add("Success");
            }
            else
            {
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add("Failure");
            }

            return rowAffected;

            #region
            //transaction = new TransactionalInformation();
            //dbConnection.Configuration.ProxyCreationEnabled = false;

            //if (transaction != null)
            //{
            //    dbConnection.Transaction.Add(transaction);
            //    int rowAffected = dbConnection.SaveChanges();

            //    var _tb = (from tb in dbConnection.Transaction
            //              orderby tb.TransactionId
            //              select tb).Take(1).FirstOrDefault();

            //    _tb.CreatedBy = _tb.TransactionId;
            //    _tb.LastModifiedBy = _tb.TransactionId;

            //    dbConnection.Entry(_tb).Property(t => t.CreatedBy).IsModified = true;
            //    dbConnection.Entry(_tb).Property(t => t.LastModifiedBy).IsModified = true;
            //    rowAffected=dbConnection.SaveChanges();

            //    if (rowAffected > 0)
            //    {
            //        transaction.ReturnStatus = true;
            //        transaction.ReturnMessage.Add("Tissue Bank is registered successfully.");
            //    }
            //    else
            //    {
            //        transaction.ReturnStatus = false;
            //        transaction.ReturnMessage.Add("Database Error");
            //    }
            //}
            #endregion
        }
    }
}

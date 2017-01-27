using Allocat.DataModel;
using System;
using System.Collections.Generic;
using System.Data;

namespace Allocat.DataServiceInterface
{
    public interface ITransactionDataService : IDataService, IDisposable
    {
        int Transaction_Create(DateTime TransactionInitiateDate, float Amount, int UserId,string RequestBody, out TransactionalInformation transaction);
        int Transaction_Update(int TissueBankId, int TransactionId, string AuthTransactionId,string AuthCode,int StatusId,DateTime TransactionCompleteDate,string ResponseBody, out TransactionalInformation transaction);
    }
}
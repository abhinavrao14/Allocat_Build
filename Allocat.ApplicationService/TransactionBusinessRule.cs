using System;
using Allocat.DataServiceInterface;

namespace Allocat.ApplicationService
{
    public class TransactionBusinessRule : ValidationRules
    {
        ITransactionDataService transactionDataService;

        public TransactionBusinessRule(ITransactionDataService _transactionDataService)
        {
            transactionDataService = _transactionDataService;
        }

        public void ValidateTransaction_Create(DateTime TransactionInitiateDate, float Amount, int UserId, string RequestBody)
        {
           
        }

        public void ValidateTransaction_Update(string fullName, string userName, string emailId, string securityQuestion, string securityAnswer)
        {
        }
    }
}
using Allocat.DataModel;
using Allocat.DataServiceInterface;
using Allocat.Utility;
using System;
using System.Collections.Generic;

namespace Allocat.ApplicationService
{
    public class TransactionBusinessService
    {
        private ITransactionDataService transactionDataService;

        public TransactionBusinessService(ITransactionDataService _transactionDataService)
        {
            transactionDataService = _transactionDataService;
        }

        public int Transaction_Create(DateTime TransactionInitiateDate, float Amount, int UserId, string RequestBody, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            TransactionBusinessRule transactionBusinessRule = new TransactionBusinessRule(transactionDataService);
            int TransactionId = 0;
            try
            {
                transactionDataService.CreateSession();

                transactionBusinessRule.ValidateTransaction_Create(TransactionInitiateDate, Amount, UserId, RequestBody);

                if (transactionBusinessRule.ValidationStatus == true)
                {
                    //send this password on mail
                    string Password = Utility.Utilities.RandomAlphaNumeric(6);

                    TransactionId=transactionDataService.Transaction_Create(TransactionInitiateDate, Amount, UserId, RequestBody, out transaction);
                }
                else
                {
                    transaction.ReturnStatus = transactionBusinessRule.ValidationStatus;
                    transaction.ReturnMessage = transactionBusinessRule.ValidationMessage;
                    transaction.ValidationErrors = transactionBusinessRule.ValidationErrors;
                }

            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                transactionDataService.CloseSession();
            }
            return TransactionId;
        }

        //public void Transaction_Update(string FullName, string UserName, string EmailId, string SecurityQuestion, string SecurityAnswer, out TransactionalInformation transaction)
        //{
        //    transaction = new TransactionalInformation();
        //    TransactionBusinessRule tbBusinessRule = new TransactionBusinessRule(transactionDataService);

        //    try
        //    {
        //        transactionDataService.CreateSession();

        //        tbBusinessRule.ValidateTransaction_Update(FullName, UserName, EmailId, SecurityQuestion, SecurityAnswer);

        //        if (tbBusinessRule.ValidationStatus == true)
        //        {
        //            string Password = Utilities.RandomAlphaNumeric(6);

        //            transactionDataService.Transaction_Update(FullName, UserName, EmailId, SecurityQuestion, SecurityAnswer,Password, out transaction);

        //            if (transaction.ReturnStatus == true)
        //            {
        //                //Send an email with generated Password --BHASKAR SIR EMAIL SERVICE--
        //            }
        //        }
        //        else
        //        {
        //            transaction.ReturnStatus = tbBusinessRule.ValidationStatus;
        //            transaction.ReturnMessage = tbBusinessRule.ValidationMessage;
        //            transaction.ValidationErrors = tbBusinessRule.ValidationErrors;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        transaction.ReturnMessage = new List<string>();
        //        string errorMessage = ex.Message;
        //        transaction.ReturnStatus = false;
        //        transaction.ReturnMessage.Add(errorMessage);
        //    }
        //    finally
        //    {
        //        transactionDataService.CloseSession();
        //    }
        //}
    }
}

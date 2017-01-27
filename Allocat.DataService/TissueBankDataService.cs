using Allocat.DataModel;
using Allocat.DataServiceInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Allocat.DataService
{
    public class TissueBankDataService : EntityFrameworkDataService, ITissueBankDataService
    {
        public int TissueBank_User_Registration(string FullName, string UserName, string EmailId, string SecurityQuestion, string SecurityAnswer, string Password, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            int rowAffected = 0;

            var parameterFullName = new SqlParameter("@FullName", SqlDbType.VarChar);
            parameterFullName.Value = FullName;

            var parameterUserName = new SqlParameter("@UserName", SqlDbType.NVarChar);
            parameterUserName.Value = UserName;

            var parameterEmailId = new SqlParameter("@EmailId", SqlDbType.NVarChar);
            parameterEmailId.Value = EmailId;

            var parameterSecurityQuestion = new SqlParameter("@SecurityQuestion", SqlDbType.VarChar);
            parameterSecurityQuestion.Value = SecurityQuestion;

            var parameterSecurityAnswer = new SqlParameter("@SecurityAnswer", SqlDbType.VarChar);
            parameterSecurityAnswer.Value = SecurityAnswer;

            var parameterPassword = new SqlParameter("@Password", SqlDbType.VarChar);
            parameterPassword.Value = Password;

            rowAffected = dbConnection.Database.ExecuteSqlCommand("exec dbo.usp_TissueBank_User_Registration @FullName, @UserName, @EmailId, @SecurityQuestion, @SecurityAnswer, @Password", parameterFullName, parameterUserName, parameterEmailId, parameterSecurityQuestion, parameterSecurityAnswer, parameterPassword);

            if (rowAffected > 0)
            {
                transaction.ReturnStatus = true;
                transaction.ReturnMessage.Add("Operation Successfull.");
            }
            else
            {
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add("Database Error");
            }

            return rowAffected;

        }

        public void TissueBank_Add(string TissueBankName, string ContactPersonFirstName, string ContactPersonLastName, string ContactPersonNumber, string ContactPersonEmailId, string FaxNumber, string TissueBankEmailId, string BusinessURL, string TissueBankAddress, int CityId, string ZipCode, string TissueBankStateLicense, string AATBLicenseNumber, DateTime AATBExpirationDate, DateTime AATBAccredationDate, string CreditCardNumber, string CustomerProfileId, string CustomerPaymentProfileIds, string BillingAddress, int BillingCityId, string BillingZipCode, string BillingFaxNumber, string BillingEmailId, string BillingContactNumber, int UserId, int TissueBankId, int TransactionId, string AuthTransactionId, string AuthCode, int StatusId, DateTime TransactionCompleteDate, string ResponseBody, out TransactionalInformation transaction)
        {

            transaction = new TransactionalInformation();
            var lstMessage = dbConnection.usp_TissueBank_Add(TissueBankName, ContactPersonFirstName, ContactPersonLastName, ContactPersonNumber, ContactPersonEmailId, FaxNumber, TissueBankEmailId, BusinessURL, TissueBankAddress, CityId, ZipCode, TissueBankStateLicense, AATBLicenseNumber, AATBExpirationDate, AATBAccredationDate, CreditCardNumber, CustomerProfileId, CustomerPaymentProfileIds, BillingAddress, BillingCityId, BillingZipCode, BillingFaxNumber, BillingEmailId, BillingContactNumber, UserId, TissueBankId, TransactionId, AuthTransactionId, AuthCode, StatusId, TransactionCompleteDate, ResponseBody);

            var Message = lstMessage.FirstOrDefault();
            transaction.ReturnMessage.Add(Message.ToString());
            transaction.ReturnStatus = true;

        }

        public usp_TissueBank_Get_Result GetTissueBankById(int TissueBankId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            IEnumerable<usp_TissueBank_Get_Result> lstTB = dbConnection.usp_TissueBank_Get(TissueBankId);

            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add("TissueBank found.");

            return lstTB.FirstOrDefault();
        }

        public void TissueBank_Update(string TissueBankName, string ContactPersonFirstName, string ContactPersonLastName, string ContactPersonNumber, string ContactPersonEmailId, string FaxNumber, string TissueBankEmailId, string BusinessURL, string TissueBankAddress, int CityId, string ZipCode, string CustomerServiceLandLineNumber, string TaxPayerId, string TissueBankStateLicense, string AATBLicenseNumber, DateTime AATBExpirationDate, DateTime AATBAccredationDate, string CreditCardNumber, string CustomerProfileId, string CustomerPaymentProfileIds, string BillingAddress, int BillingCityId, string BillingZipCode, string BillingFaxNumber, string BillingEmailId, string BillingContactNumber, int UserId, int TissueBankId, int TransactionId, string AuthTransactionId, string AuthCode, int StatusId, DateTime TransactionCompleteDate, string ResponseBody, string OperationType, out TransactionalInformation transaction)
        {
            string ReturnMessage = "";
            transaction = new TransactionalInformation();

            if (OperationType == "UpdateTissueBankDetail")
            {
                var lstMessage = dbConnection.usp_TissueBank_UpdateTissueBankDetail(TissueBankName, ContactPersonEmailId, TissueBankEmailId, BusinessURL, TissueBankAddress, CityId, ZipCode, TissueBankStateLicense, CustomerServiceLandLineNumber, FaxNumber,TaxPayerId, AATBLicenseNumber, AATBExpirationDate, AATBAccredationDate, UserId, TissueBankId);
                ReturnMessage= lstMessage.FirstOrDefault();
            }
            else
            {
                var lstMessage = dbConnection.usp_TissueBank_UpdateTissueBankBillingDetail(ContactPersonFirstName, ContactPersonLastName, BillingAddress, BillingCityId, BillingZipCode, BillingFaxNumber, BillingEmailId, BillingContactNumber, CreditCardNumber, UserId, TissueBankId);
                ReturnMessage = lstMessage.FirstOrDefault();
            }

            transaction.ReturnMessage.Add(ReturnMessage);

            if (ReturnMessage.Contains("success"))
            {
                transaction.ReturnStatus = true;
            }
            else
            {
                transaction.ReturnStatus = false;
            }
        }

        public bool ValidateUniqueTissueBankEmailId(string TissueBankEmailId)
        {
            TissueBank tissueBank = dbConnection.TissueBank.FirstOrDefault(c => c.TissueBankEmailId == TissueBankEmailId);
            if (tissueBank == null)
                return true;

            return false;
        }

        public bool ValidateUniqueContactPersonNumber(string ContactPersonNumber)
        {
            TissueBank tissueBank = dbConnection.TissueBank.FirstOrDefault(c => c.ContactPersonNumber == ContactPersonNumber);
            if (tissueBank == null)
                return true;

            return false;
        }

        public bool ValidateUniqueAATBLicenseNumber(string AATBLicenseNumber)
        {
            TissueBank tissueBank = dbConnection.TissueBank.FirstOrDefault(c => c.AATBLicenseNumber == AATBLicenseNumber);
            if (tissueBank == null)
                return true;

            return false;
        }

        public bool ValidateUniqueTissueBankStateLicense(string TissueBankStateLicense)
        {
            TissueBank tissueBank = dbConnection.TissueBank.FirstOrDefault(c => c.TissueBankStateLicense == TissueBankStateLicense);
            if (tissueBank == null)
                return true;

            return false;
        }

        public bool ValidateSingleTissueBankEmailId(string TissueBankEmailId, int TissueBankId)
        {
            TissueBank tissueBank = dbConnection.TissueBank.FirstOrDefault(c => c.TissueBankEmailId == TissueBankEmailId && c.TissueBankId!= TissueBankId);
            if (tissueBank == null)
                return true;

            return false;
        }

        public bool ValidateSingleContactPersonNumber(string ContactPersonNumber, int TissueBankId)
        {
            TissueBank tissueBank = dbConnection.TissueBank.FirstOrDefault(c => c.ContactPersonNumber == ContactPersonNumber && c.TissueBankId != TissueBankId);
            if (tissueBank == null)
                return true;

            return false;
        }

        public bool ValidateSingleAATBLicenseNumber(string AATBLicenseNumber, int TissueBankId)
        {
            TissueBank tissueBank = dbConnection.TissueBank.FirstOrDefault(c => c.AATBLicenseNumber == AATBLicenseNumber && c.TissueBankId != TissueBankId);
            if (tissueBank == null)
                return true;

            return false;
        }

        public bool ValidateSingleTissueBankStateLicense(string TissueBankStateLicense, int TissueBankId)
        {
            TissueBank tissueBank = dbConnection.TissueBank.FirstOrDefault(c => c.TissueBankStateLicense == TissueBankStateLicense && c.TissueBankId != TissueBankId);
            if (tissueBank == null)
                return true;

            return false;
        }

        //public string UpdateTb(TissueBank tissueBank)
        //{
        //    using (AllocatDbEntities db = new AllocatDbEntities())
        //    {
        //        try
        //        {
        //            db.Entry(tissueBank).State = System.Data.Entity.EntityState.Modified;
        //            db.SaveChanges();
        //            return "OK";
        //        }
        //        catch (Exception ee)
        //        {
        //            return ee.Message;
        //        }
        //    }
        //}

        //public string DeleteTissueBank(int TissueBankId)
        //{
        //    try
        //    {
        //        int _TbId = TissueBankId;
        //        using (AllocatDbEntities db = new AllocatDbEntities())
        //        {
        //            var tissueBank = db.TissueBank.Find(_TbId);

        //            tissueBank.IsActive = false;
        //            db.Entry(tissueBank).Property(t => t.IsActive).IsModified = true;
        //            db.SaveChanges();
        //            return "OK";
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        return ee.Message;
        //    }
        //}
    }
}

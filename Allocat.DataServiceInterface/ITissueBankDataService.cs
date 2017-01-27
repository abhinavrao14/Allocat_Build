using Allocat.DataModel;
using System;
using System.Collections.Generic;
using System.Data;

namespace Allocat.DataServiceInterface
{
    public interface ITissueBankDataService : IDataService, IDisposable
    {
        int TissueBank_User_Registration(string FullName, string UserName, string EmailId, string SecurityQuestion, string SecurityAnswer, string Password, out TransactionalInformation transaction);
        void TissueBank_Add(string TissueBankName, string ContactPersonFirstName,string ContactPersonLastName, string ContactPersonNumber, string ContactPersonEmailId, string FaxNumber, string TissueBankEmailId, string BusinessURL, string TissueBankAddress, int CityId, string ZipCode, string TissueBankStateLicense, string AATBLicenseNumber, DateTime AATBExpirationDate, DateTime AATBAccredationDate, string CreditCardNumber, string CustomerProfileId, string CustomerPaymentProfileIds, string BillingAddress, int BillingCityId, string BillingZipCode, string BillingFaxNumber, string BillingEmailId, string BillingContactNumber, int UserId, int TissueBankId, int TransactionId, string AuthTransactionId, string AuthCode, int StatusId, DateTime TransactionCompleteDate, string ResponseBody, out TransactionalInformation transaction);
        void TissueBank_Update(string TissueBankName, string ContactPersonFirstName,string ContactPersonLastName, string ContactPersonNumber, string ContactPersonEmailId, string FaxNumber, string TissueBankEmailId, string BusinessURL, string TissueBankAddress, int CityId, string ZipCode, string CustomerServiceLandLineNumber ,string TaxPayerId,string TissueBankStateLicense, string AATBLicenseNumber, DateTime AATBExpirationDate, DateTime AATBAccredationDate, string CreditCardNumber, string CustomerProfileId, string CustomerPaymentProfileIds, string BillingAddress, int BillingCityId, string BillingZipCode, string BillingFaxNumber, string BillingEmailId, string BillingContactNumber, int UserId, int TissueBankId, int TransactionId, string AuthTransactionId, string AuthCode, int StatusId, DateTime TransactionCompleteDate, string ResponseBody,string OperationType, out TransactionalInformation transaction);
        usp_TissueBank_Get_Result GetTissueBankById(int TissueBankId, out TransactionalInformation transaction);
        bool ValidateUniqueTissueBankEmailId(string TissueBankEmailId);
        bool ValidateUniqueContactPersonNumber(string ContactPersonNumber);
        bool ValidateUniqueAATBLicenseNumber(string AATBLicenseNumber);
        bool ValidateUniqueTissueBankStateLicense(string TissueBankStateLicense);

        bool ValidateSingleTissueBankEmailId(string TissueBankEmailId,int TissueBankId);
        bool ValidateSingleContactPersonNumber(string ContactPersonNumber, int TissueBankId);
        bool ValidateSingleAATBLicenseNumber(string AATBLicenseNumber, int TissueBankId);
        bool ValidateSingleTissueBankStateLicense(string TissueBankStateLicense, int TissueBankId);
    }
}

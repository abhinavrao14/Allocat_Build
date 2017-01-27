using Allocat.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allocat.WebApi.WebApiModel
{
    public class TissueBankApiModel : TransactionalInformation
    {
        public usp_TissueBank_Get_Result tissueBank;
    }

    public class TissueBankAdd_DTO
    {
        public string TissueBankName { get; set; }
        public string ContactPersonFirstName { get; set; }
        public string ContactPersonLastName { get; set; }
        public string ContactPersonNumber { get; set; }
        public string ContactPersonEmailId { get; set; }
        public string FaxNumber { get; set; }
        public string TissueBankEmailId { get; set; }
        public string BusinessURL { get; set; }
        public string TissueBankAddress { get; set; }
        public int CityId { get; set; }
        public string ZipCode { get; set; }
        public string TissueBankStateLicense { get; set; }
        public string AATBLicenseNumber { get; set; }
        public DateTime AATBExpirationDate { get; set; }
        public DateTime AATBAccredationDate { get; set; }
        public string CreditCardNumber { get; set; }
        public int CreditCardType { get; set; }
        public string ExpiryDate { get; set; }
        public string CardCode { get; set; }
        public string CustomerProfileId { get; set; }
        public string CustomerPaymentProfileIds { get; set; }
        public string BillingAddress { get; set; }
        public int BillingCityId { get; set; }
        public string BillingZipCode { get; set; }
        public string BillingFaxNumber { get; set; }
        public string BillingEmailId { get; set; }
        public string BillingContactNumber { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public int UserId { get; set; }
        public int TissueBankId { get; set; }

        public int TransactionId { get; set; }
        public string AuthTransactionId { get; set; }
        public string AuthCode { get; set; }
        public int StatusId { get; set; }
        public DateTime TransactionCompleteDate { get; set; }
        public string ResponseBody { get; set; }
    }

    public class TissueBankSignUp_DTO
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string EmailId { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
    }

    public class TissueBankUpdate_DTO
    {
        public string TissueBankName { get; set; }
        public string ContactPersonFirstName { get; set; }
        public string ContactPersonLastName { get; set; }
        public string ContactPersonNumber { get; set; }
        public string ContactPersonEmailId { get; set; }
        public string FaxNumber { get; set; }
        public string TissueBankEmailId { get; set; }
        public string BusinessURL { get; set; }
        public string TissueBankAddress { get; set; }
        public int CityId { get; set; }
        public string ZipCode { get; set; }
        public string CustomerServiceLandLineNumber { get; set; }
        public string TaxPayerId { get; set; }
        public string TissueBankStateLicense { get; set; }
        public string AATBLicenseNumber { get; set; }
        public DateTime AATBExpirationDate { get; set; }
        public DateTime AATBAccredationDate { get; set; }
        public string CreditCardNumber { get; set; }
        public int CreditCardType { get; set; }
        public string ExpiryDate { get; set; }
        public string CardCode { get; set; }
        public string CustomerProfileId { get; set; }
        public string CustomerPaymentProfileIds { get; set; }
        public string BillingAddress { get; set; }
        public int BillingCityId { get; set; }
        public string BillingZipCode { get; set; }
        public string BillingFaxNumber { get; set; }
        public string BillingEmailId { get; set; }
        public string BillingContactNumber { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public int BillingStateId { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public int UserId { get; set; }
        public int TissueBankId { get; set; }

        public int TransactionId { get; set; }
        public string AuthTransactionId { get; set; }
        public string AuthCode { get; set; }
        public int StatusId { get; set; }
        public DateTime TransactionCompleteDate { get; set; }
        public string ResponseBody { get; set; }
        public string OperationType { get; set; }
    }
}
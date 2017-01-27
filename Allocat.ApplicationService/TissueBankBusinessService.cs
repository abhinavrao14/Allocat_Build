using Allocat.DataModel;
using Allocat.DataServiceInterface;
using Allocat.Utility;
using System;
using System.Collections.Generic;
using Allocat.ApplicationService.EmailService;

namespace Allocat.ApplicationService
{
    public class TissueBankBusinessService
    {
        private ITissueBankDataService tbDataService;

        public TissueBankBusinessService(ITissueBankDataService _tbDataService)
        {
            tbDataService = _tbDataService;
        }

        public void TissueBank_Add(string TissueBankName, string ContactPersonFirstName, string ContactPersonLastName, string ContactPersonNumber, string ContactPersonEmailId, string FaxNumber, string TissueBankEmailId, string BusinessURL, string TissueBankAddress, int CityId, string ZipCode, string TissueBankStateLicense, string AATBLicenseNumber, DateTime AATBExpirationDate, DateTime AATBAccredationDate, string CreditCardNumber, int CreditCardType, string ExpiryDate, string CardCode, string CustomerProfileId, string CustomerPaymentProfileIds, string BillingAddress, int BillingCityId, string BillingZipCode, string BillingFaxNumber, string BillingEmailId, string BillingContactNumber, string BillingCity, string BillingState, string State, string City, int UserId, int TissueBankId, int TransactionId, string AuthTransactionId, string AuthCode, int StatusId, DateTime TransactionCompleteDate, string ResponseBody,out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            TissueBankBusinessRule tbBusinessRule = new TissueBankBusinessRule(tbDataService);

            try
            {
                tbDataService.CreateSession();

                tbBusinessRule.ValidateTissueBank_Add(TissueBankName, ContactPersonFirstName, ContactPersonLastName, ContactPersonNumber, ContactPersonEmailId, FaxNumber, TissueBankEmailId, BusinessURL, TissueBankAddress, CityId, ZipCode, TissueBankStateLicense, AATBLicenseNumber, AATBExpirationDate, AATBAccredationDate, CreditCardNumber, CreditCardType, ExpiryDate, CardCode, CustomerProfileId, CustomerPaymentProfileIds, BillingAddress, BillingCityId, BillingZipCode, BillingFaxNumber, BillingEmailId, BillingContactNumber, BillingCity, BillingState, State, City, UserId, TissueBankId, TransactionId, AuthTransactionId, AuthCode, StatusId, TransactionCompleteDate, ResponseBody);
                if (tbBusinessRule.ValidationStatus == true)
                {
                    //send this password on mail
                    string Password = Utility.Utilities.RandomAlphaNumeric(6);

                    tbDataService.TissueBank_Add(TissueBankName, ContactPersonFirstName,ContactPersonLastName, ContactPersonNumber, ContactPersonEmailId, FaxNumber, TissueBankEmailId, BusinessURL, TissueBankAddress, CityId, ZipCode, TissueBankStateLicense, AATBLicenseNumber, AATBExpirationDate, AATBAccredationDate, CreditCardNumber, CustomerProfileId, CustomerPaymentProfileIds, BillingAddress, BillingCityId, BillingZipCode, BillingFaxNumber, BillingEmailId, BillingContactNumber, UserId, TissueBankId, TransactionId, AuthTransactionId, AuthCode, StatusId, TransactionCompleteDate, ResponseBody, out transaction);
                }
                else
                {
                    transaction.ReturnStatus = tbBusinessRule.ValidationStatus;
                    transaction.ReturnMessage = tbBusinessRule.ValidationMessage;
                    transaction.ValidationErrors = tbBusinessRule.ValidationErrors;
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
                tbDataService.CloseSession();
            }
        }

        public void TissueBank_User_Registration(string FullName, string UserName, string EmailId, string SecurityQuestion, string SecurityAnswer, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            TissueBankBusinessRule tbBusinessRule = new TissueBankBusinessRule(tbDataService);

            try
            {
                tbDataService.CreateSession();

                tbBusinessRule.ValidateTissueBankUserRegistration(FullName, UserName, EmailId, SecurityQuestion, SecurityAnswer);

                if (tbBusinessRule.ValidationStatus == true)
                {
                    string Password = Utilities.RandomAlphaNumeric(6);

                    tbDataService.TissueBank_User_Registration(FullName, UserName, EmailId, SecurityQuestion, SecurityAnswer, Password, out transaction);

                    if (transaction.ReturnStatus == true)
                    {
                        //Send an email with generated Password --BHASKAR SIR EMAIL SERVICE--
                        Email email = new Email();
                        //get emailBody from email body type class
                        email.SendEmail(EmailId);
                    }
                }
                else
                {
                    transaction.ReturnStatus = tbBusinessRule.ValidationStatus;
                    transaction.ReturnMessage = tbBusinessRule.ValidationMessage;
                    transaction.ValidationErrors = tbBusinessRule.ValidationErrors;
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
                tbDataService.CloseSession();
            }
        }

        public usp_TissueBank_Get_Result GetTissueBankById(int TissueBankId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            usp_TissueBank_Get_Result tissueBank= null;

            try
            {
                tbDataService.CreateSession();
                tissueBank = tbDataService.GetTissueBankById(TissueBankId, out transaction);
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
                tbDataService.CloseSession();
            }

            return tissueBank;

        }

        public void TissueBank_Update(string TissueBankName, string ContactPersonFirstName, string ContactPersonLastName, string ContactPersonNumber, string ContactPersonEmailId, string FaxNumber, string TissueBankEmailId, string BusinessURL, string TissueBankAddress, int CityId, string ZipCode, string CustomerServiceLandLineNumber, string TaxPayerId, string TissueBankStateLicense, string AATBLicenseNumber, DateTime AATBExpirationDate, DateTime AATBAccredationDate, string CreditCardNumber, string CustomerProfileId, string CustomerPaymentProfileIds, string BillingAddress, int BillingCityId, string BillingZipCode, string BillingFaxNumber, string BillingEmailId, string BillingContactNumber, int UserId, int TissueBankId, int TransactionId, string AuthTransactionId, string AuthCode, int StatusId, DateTime TransactionCompleteDate, string ResponseBody, string OperationType, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            TissueBankBusinessRule tbBusinessRule = new TissueBankBusinessRule(tbDataService);

            try
            {
                tbDataService.CreateSession();

                tbBusinessRule.ValidateTissueBank_Update(TissueBankName, ContactPersonFirstName, ContactPersonLastName, ContactPersonNumber, ContactPersonEmailId, FaxNumber, TissueBankEmailId, BusinessURL, TissueBankAddress, CityId, ZipCode, CustomerServiceLandLineNumber, TaxPayerId, TissueBankStateLicense, AATBLicenseNumber, AATBExpirationDate, AATBAccredationDate, CreditCardNumber, CustomerProfileId, CustomerPaymentProfileIds, BillingAddress, BillingCityId, BillingZipCode, BillingFaxNumber, BillingEmailId, BillingContactNumber, UserId, TissueBankId, TransactionId, AuthTransactionId, AuthCode, StatusId, TransactionCompleteDate, ResponseBody, OperationType);
                if (tbBusinessRule.ValidationStatus == true)
                {
                    //send this password on mail
                    string Password = Utility.Utilities.RandomAlphaNumeric(6);

                    tbDataService.TissueBank_Update(TissueBankName, ContactPersonFirstName, ContactPersonLastName, ContactPersonNumber, ContactPersonEmailId, FaxNumber, TissueBankEmailId, BusinessURL, TissueBankAddress, CityId, ZipCode, CustomerServiceLandLineNumber, TaxPayerId, TissueBankStateLicense, AATBLicenseNumber,  AATBExpirationDate,  AATBAccredationDate, CreditCardNumber, CustomerProfileId, CustomerPaymentProfileIds, BillingAddress, BillingCityId, BillingZipCode, BillingFaxNumber, BillingEmailId, BillingContactNumber, UserId, TissueBankId, TransactionId, AuthTransactionId, AuthCode, StatusId,  TransactionCompleteDate, ResponseBody, OperationType, out transaction);
                }
                else
                {
                    transaction.ReturnStatus = tbBusinessRule.ValidationStatus;
                    transaction.ReturnMessage = tbBusinessRule.ValidationMessage;
                    transaction.ValidationErrors = tbBusinessRule.ValidationErrors;
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
                tbDataService.CloseSession();
            }
        }

    }
}

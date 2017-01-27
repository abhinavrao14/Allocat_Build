using Allocat.ApplicationService;
using Allocat.DataModel;
using Allocat.DataService;
using Allocat.DataServiceInterface;
using Allocat.WebApi.WebApiModel;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Allocat.WebApi.CustomService;
using System;

namespace Allocat.WebApi.Controllers
{
    public class TissueBankController : ApiController
    {
        ITissueBankDataService tbDataService;
        ITransactionDataService transactionDataService;
        IErrorDataService errorDataService;
        IStatusDataService statusDataService;
        IStateDataService stateDataService;
        ICityDataService cityDataService;

        public TissueBankController()
        {
            tbDataService = new TissueBankDataService();
            transactionDataService = new TransactionDataService();
            errorDataService = new ErrorDataService();
            statusDataService = new StatusDataService();
            stateDataService = new StateDataService();
            cityDataService = new CityDataService();
        }

        public HttpResponseMessage Get([FromUri] int TissueBankId)
        {
            TissueBankApiModel tbApiModel = new TissueBankApiModel();
            TransactionalInformation transaction = new TransactionalInformation();

            TissueBankBusinessService tissueBankBusinessService = new TissueBankBusinessService(tbDataService);

            usp_TissueBank_Get_Result tissueBank = tissueBankBusinessService.GetTissueBankById
                (TissueBankId, out transaction);

            tbApiModel.tissueBank = tissueBank;
            tbApiModel.ReturnStatus = transaction.ReturnStatus;
            tbApiModel.ReturnMessage = transaction.ReturnMessage;
            tbApiModel.IsAuthenicated = true;

            if (transaction.ReturnStatus == true)
            {
                var response = Request.CreateResponse<TissueBankApiModel>(HttpStatusCode.OK, tbApiModel);
                return response;
            }

            var badResponse = Request.CreateResponse<TissueBankApiModel>(HttpStatusCode.BadRequest, tbApiModel);
            return badResponse;
        }

        [HttpPost]
        public HttpResponseMessage Post(TissueBankAdd_DTO tissueBankAdd_DTO)
        {
            //Initialisation
            TransactionalInformation transaction = new TransactionalInformation();
            TissueBankApiModel tbApiModel = new TissueBankApiModel();
            Status status = new Status();

            TissueBankBusinessService tissueBankBusinessService = new TissueBankBusinessService(tbDataService);
            TransactionBusinessService transactionBusinessService = new TransactionBusinessService(transactionDataService);
            ErrorBusinessService errorBusinessService = new ErrorBusinessService(errorDataService);
            StatusBusinessService statusBusinessService = new StatusBusinessService(statusDataService);

            //Convert object to string to send as requestBody
            string objToPass = Utility.Utilities.SerializeObject<TissueBankAdd_DTO>(tissueBankAdd_DTO);

            //create transaction
            int TransactionId = transactionBusinessService.Transaction_Create(DateTime.Now, 25, tissueBankAdd_DTO.UserId, objToPass, out transaction);

            //Cutting $25 for registration
            var response = CreateCustomerProfileAndCharge(tissueBankAdd_DTO);

            // if response is not null then only save tissue bank detail in database and update transaction too.
            if (response.CustomerProfileId != null)
            {
                //get status from database for Success
                status = statusBusinessService.GetStatusByStatusName("Success");

                #region static data of response from authorize .net
                //tissueBankAdd_DTO.AuthTransactionId = "123454613";
                //tissueBankAdd_DTO.CustomerProfileId = "56456123132";
                //tissueBankAdd_DTO.CustomerPaymentProfileIds = "456123187";
                //tissueBankAdd_DTO.AuthCode = "456456";
                //tissueBankAdd_DTO.ResponseBody = "";
                #endregion

                tissueBankAdd_DTO.AuthTransactionId = response.Transaction.TransactionId;
                tissueBankAdd_DTO.CustomerProfileId = response.CustomerProfileId;
                tissueBankAdd_DTO.CustomerPaymentProfileIds = response.CustomerPaymentProfileIds[0];
                tissueBankAdd_DTO.AuthCode = response.Transaction.AuthCode;
                 
                tissueBankAdd_DTO.StatusId = status.StatusId;
                tissueBankAdd_DTO.TransactionCompleteDate = DateTime.Now;
                tissueBankAdd_DTO.TransactionId = TransactionId;

                //removing card detail from object and then storing response detail in db
                tissueBankAdd_DTO.CreditCardNumber = tissueBankAdd_DTO.CreditCardNumber;
                tissueBankAdd_DTO.CreditCardType = 0;
                tissueBankAdd_DTO.CardCode = "";
                tissueBankAdd_DTO.ExpiryDate = "";

                tissueBankAdd_DTO.ResponseBody = Utility.Utilities.SerializeObject<ResCustomerProfile>(response);

                //add tissue bank
                tissueBankBusinessService.TissueBank_Add(tissueBankAdd_DTO.TissueBankName, tissueBankAdd_DTO.ContactPersonFirstName, tissueBankAdd_DTO.ContactPersonLastName, tissueBankAdd_DTO.ContactPersonNumber, tissueBankAdd_DTO.ContactPersonEmailId, tissueBankAdd_DTO.FaxNumber, tissueBankAdd_DTO.TissueBankEmailId, tissueBankAdd_DTO.BusinessURL, tissueBankAdd_DTO.TissueBankAddress, tissueBankAdd_DTO.CityId, tissueBankAdd_DTO.ZipCode, tissueBankAdd_DTO.TissueBankStateLicense, tissueBankAdd_DTO.AATBLicenseNumber, tissueBankAdd_DTO.AATBExpirationDate, tissueBankAdd_DTO.AATBAccredationDate, tissueBankAdd_DTO.CreditCardNumber, tissueBankAdd_DTO.CreditCardType, tissueBankAdd_DTO.ExpiryDate, tissueBankAdd_DTO.CardCode, tissueBankAdd_DTO.CustomerProfileId, tissueBankAdd_DTO.CustomerPaymentProfileIds, tissueBankAdd_DTO.BillingAddress, tissueBankAdd_DTO.BillingCityId, tissueBankAdd_DTO.BillingZipCode, tissueBankAdd_DTO.BillingFaxNumber, tissueBankAdd_DTO.BillingEmailId, tissueBankAdd_DTO.BillingContactNumber, tissueBankAdd_DTO.BillingCity, tissueBankAdd_DTO.BillingState, tissueBankAdd_DTO.State, tissueBankAdd_DTO.City, tissueBankAdd_DTO.UserId, tissueBankAdd_DTO.TissueBankId, tissueBankAdd_DTO.TransactionId, tissueBankAdd_DTO.AuthTransactionId, tissueBankAdd_DTO.AuthCode, tissueBankAdd_DTO.StatusId, tissueBankAdd_DTO.TransactionCompleteDate, tissueBankAdd_DTO.ResponseBody, out transaction);

                tbApiModel.ReturnMessage = transaction.ReturnMessage;
                tbApiModel.ReturnStatus = transaction.ReturnStatus;
            }
            else
            {
                //get status from database for Success
                status = statusBusinessService.GetStatusByStatusName("Error");

                //if response is null then log error and update transaction too.
                string errorMessage = errorBusinessService.Error_Create(status.StatusId, response.Message, "", TransactionId, tissueBankAdd_DTO.UserId, response.MessageCode);

                tbApiModel.ReturnStatus = transaction.ReturnStatus = false;
                tbApiModel.ReturnMessage.Add(response.Message);
            }

            if (transaction.ReturnStatus == false)
            {
                tbApiModel.ValidationErrors = transaction.ValidationErrors;
                return Request.CreateResponse<TissueBankApiModel>(HttpStatusCode.BadRequest, tbApiModel);
            }
            else
            {
                return Request.CreateResponse<TissueBankApiModel>(HttpStatusCode.OK, tbApiModel);
            }
        }

        private ResCustomerProfile CreateCustomerProfileAndCharge(TissueBankAdd_DTO tissueBankAdd_DTO)
        {
            CustomService.AllocatCustomServiceClient obj = new AllocatCustomServiceClient();
            Customer objCustomer = new Customer();
            ResCustomerProfile response = new ResCustomerProfile();
            CreditCard credit = new CreditCard();
            AddressInfo address = new AddressInfo();
            // Setting input data

            credit.CreditCardNumber = tissueBankAdd_DTO.CreditCardNumber;
            credit.CreditCardType = tissueBankAdd_DTO.CreditCardType;
            credit.CardCode = tissueBankAdd_DTO.ExpiryDate;
            credit.ExpiryDate = tissueBankAdd_DTO.ExpiryDate;
            objCustomer.CardInfo = credit;

            objCustomer.EmailId = tissueBankAdd_DTO.ContactPersonEmailId;
            objCustomer.LastName = tissueBankAdd_DTO.ContactPersonLastName; 
            objCustomer.FirstName = tissueBankAdd_DTO.ContactPersonFirstName; 

            address.Address = tissueBankAdd_DTO.TissueBankAddress;
            address.City = tissueBankAdd_DTO.City;
            address.Company = tissueBankAdd_DTO.TissueBankName;
            address.Country = "US";
            address.ZipCode = tissueBankAdd_DTO.ZipCode;
            address.Email = tissueBankAdd_DTO.TissueBankEmailId;
            address.FaxNumber = tissueBankAdd_DTO.FaxNumber;
            address.FirstName = tissueBankAdd_DTO.ContactPersonFirstName;
            address.LastName = tissueBankAdd_DTO.ContactPersonLastName;
            address.PhoneNumber = tissueBankAdd_DTO.ContactPersonNumber;
            address.State = tissueBankAdd_DTO.State;
            objCustomer.HomeAddress = address;

            address = null;
            address = new AddressInfo();

            address.Address = tissueBankAdd_DTO.BillingAddress;
            address.City = tissueBankAdd_DTO.BillingCity;
            address.Company = tissueBankAdd_DTO.TissueBankName;
            address.Country = "US";
            address.ZipCode = tissueBankAdd_DTO.BillingZipCode;
            address.Email = tissueBankAdd_DTO.BillingEmailId;
            address.FaxNumber = tissueBankAdd_DTO.BillingFaxNumber;
            address.FirstName = tissueBankAdd_DTO.ContactPersonFirstName;
            address.LastName = tissueBankAdd_DTO.ContactPersonLastName;
            address.PhoneNumber = tissueBankAdd_DTO.BillingContactNumber;
            address.State = tissueBankAdd_DTO.BillingState;
            objCustomer.OfficeAddress = address;

            objCustomer.PaymentType = PaymentType.CreditCard;

            // calling service method
            response = obj.RegisterCustomerAndChargeProfile(objCustomer, 25);
            return response;
        }

        [HttpPut]
        public HttpResponseMessage Put(TissueBankUpdate_DTO tissueBankUpdate_DTO)
        {
            //Initialisation
            bool AuthResponse = false;
            TransactionalInformation transaction = new TransactionalInformation();
            TissueBankApiModel tbApiModel = new TissueBankApiModel();
            Status status = new Status();

            TissueBankBusinessService tissueBankBusinessService = new TissueBankBusinessService(tbDataService);
            TransactionBusinessService transactionBusinessService = new TransactionBusinessService(transactionDataService);
            ErrorBusinessService errorBusinessService = new ErrorBusinessService(errorDataService);
            StatusBusinessService statusBusinessService = new StatusBusinessService(statusDataService);
            StateBusinessService stateBusinessService = new StateBusinessService(stateDataService);
            CityBusinessService cityBusinessService = new CityBusinessService(cityDataService);

            //get state
            TransactionalInformation TempTransaction = new TransactionalInformation();
            State BillingState = stateBusinessService.GetStateById(tissueBankUpdate_DTO.BillingStateId, out TempTransaction);
            City BillingCity = cityBusinessService.GetCityById(tissueBankUpdate_DTO.BillingCityId, out TempTransaction);
            tissueBankUpdate_DTO.BillingCity = BillingCity.CityName;
            tissueBankUpdate_DTO.BillingState = BillingState.StateName;


            // if response is not null then only save tissue bank detail in database and update transaction too.
            if (tissueBankUpdate_DTO.OperationType == "UpdateTissueBankDetail")
            {
                AuthResponse = UpdateCustomerProfile(tissueBankUpdate_DTO);
            }
            else
            {
                AuthResponse = UpdateCustomerPaymentProfile(tissueBankUpdate_DTO);
            }

            if (AuthResponse == true)
            {
                //get status from database for Success
                status = statusBusinessService.GetStatusByStatusName("Success");

                tissueBankUpdate_DTO.CreditCardNumber = tissueBankUpdate_DTO.CreditCardNumber;
                tissueBankUpdate_DTO.CreditCardType = 0;
                tissueBankUpdate_DTO.CardCode = "";
                tissueBankUpdate_DTO.ExpiryDate = "";

                //update tissue bank
                tissueBankBusinessService.TissueBank_Update(tissueBankUpdate_DTO.TissueBankName, tissueBankUpdate_DTO.ContactPersonFirstName, tissueBankUpdate_DTO.ContactPersonLastName, tissueBankUpdate_DTO.ContactPersonNumber, tissueBankUpdate_DTO.ContactPersonEmailId, tissueBankUpdate_DTO.FaxNumber, tissueBankUpdate_DTO.TissueBankEmailId, tissueBankUpdate_DTO.BusinessURL, tissueBankUpdate_DTO.TissueBankAddress, tissueBankUpdate_DTO.CityId, tissueBankUpdate_DTO.ZipCode, tissueBankUpdate_DTO.CustomerServiceLandLineNumber, tissueBankUpdate_DTO.TaxPayerId, tissueBankUpdate_DTO.TissueBankStateLicense, tissueBankUpdate_DTO.AATBLicenseNumber, tissueBankUpdate_DTO.AATBExpirationDate, tissueBankUpdate_DTO.AATBAccredationDate, tissueBankUpdate_DTO.CreditCardNumber, tissueBankUpdate_DTO.CustomerProfileId, tissueBankUpdate_DTO.CustomerPaymentProfileIds, tissueBankUpdate_DTO.BillingAddress, tissueBankUpdate_DTO.BillingCityId, tissueBankUpdate_DTO.BillingZipCode, tissueBankUpdate_DTO.BillingFaxNumber, tissueBankUpdate_DTO.BillingEmailId, tissueBankUpdate_DTO.BillingContactNumber, tissueBankUpdate_DTO.UserId, tissueBankUpdate_DTO.TissueBankId, tissueBankUpdate_DTO.TransactionId, tissueBankUpdate_DTO.AuthTransactionId, tissueBankUpdate_DTO.AuthCode, tissueBankUpdate_DTO.StatusId, tissueBankUpdate_DTO.TransactionCompleteDate, tissueBankUpdate_DTO.ResponseBody, tissueBankUpdate_DTO.OperationType, out transaction);

                tbApiModel.ReturnMessage = transaction.ReturnMessage;
                tbApiModel.ReturnStatus = transaction.ReturnStatus;
            }
            else
            {
                tbApiModel.ReturnStatus = transaction.ReturnStatus = false;
                tbApiModel.ReturnMessage.Add("Authorize .Net operation failed.");
            }

            if (transaction.ReturnStatus == false)
            {
                tbApiModel.ValidationErrors = transaction.ValidationErrors;
                return Request.CreateResponse<TissueBankApiModel>(HttpStatusCode.BadRequest, tbApiModel);
            }
            else
            {
                return Request.CreateResponse<TissueBankApiModel>(HttpStatusCode.OK, tbApiModel);
            }
        }

        private bool UpdateCustomerProfile(TissueBankUpdate_DTO tissueBankUpdate_DTO)
        {
            CustomService.AllocatCustomServiceClient obj = new AllocatCustomServiceClient();
            // calling service method
            bool response= obj.UpdateCustomerProfile(tissueBankUpdate_DTO.CustomerProfileId, null,null, tissueBankUpdate_DTO.ContactPersonEmailId);
            return response;
        }

        private bool UpdateCustomerPaymentProfile(TissueBankUpdate_DTO tissueBankUpdate_DTO)
        {
            CustomService.AllocatCustomServiceClient obj = new AllocatCustomServiceClient();
            CustomerPaymentProfile objCustomer = new CustomerPaymentProfile();
            CreditCard credit = new CreditCard();
            AddressInfo address = new AddressInfo();

            credit.CreditCardNumber = tissueBankUpdate_DTO.CreditCardNumber;
            credit.CreditCardType = tissueBankUpdate_DTO.CreditCardType;
            credit.CardCode = tissueBankUpdate_DTO.CardCode;
            credit.ExpiryDate = tissueBankUpdate_DTO.ExpiryDate;
            objCustomer.CardInfo = credit;
            objCustomer.CustomerPaymentProfileId = tissueBankUpdate_DTO.CustomerPaymentProfileIds;
            objCustomer.CustomerProfileId = tissueBankUpdate_DTO.CustomerProfileId;
            objCustomer.isUpdateCreditCardInfo = true;

            address.Address = tissueBankUpdate_DTO.BillingAddress;
            address.City = tissueBankUpdate_DTO.BillingCity;
            address.Company = tissueBankUpdate_DTO.TissueBankName;
            address.Country = "US";
            address.ZipCode = tissueBankUpdate_DTO.BillingZipCode;
            address.Email = tissueBankUpdate_DTO.BillingEmailId;
            address.FaxNumber = tissueBankUpdate_DTO.BillingFaxNumber;
            address.FirstName = tissueBankUpdate_DTO.ContactPersonFirstName;
            address.LastName = tissueBankUpdate_DTO.ContactPersonLastName;
            address.PhoneNumber = tissueBankUpdate_DTO.BillingContactNumber;
            address.State = tissueBankUpdate_DTO.BillingState;
            objCustomer.BillTo = address;

            // calling service method
            bool response = obj.UpdateCustomerPaymentProfile(objCustomer);
            return response;
        }
    }
}

using Allocat.DataModel;
using Allocat.DataServiceInterface;
using System;
using System.Collections.Generic;

namespace Allocat.ApplicationService
{
    public class HospitalBusinessService
    {
        private IHospitalDataService hospitalDataService;

        public HospitalBusinessService(IHospitalDataService _hospitalDataService)
        {
            hospitalDataService = _hospitalDataService;
        }

        public void AddHospital(int HospitalId, string HospitalName, string ContactPersonName, string ContactPersonNumber, string HospitalEmailId, string BusinessURL, string HospitalAddress, int CityId, string RegistrationNumber, string UserName, int HospitalTypeID, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            HospitalBusinessRule hospitalBusinessRule = new HospitalBusinessRule(hospitalDataService);

            try
            {
                hospitalDataService.CreateSession();

                hospitalBusinessRule.ValidateAdd(HospitalId, HospitalName, ContactPersonName, ContactPersonNumber, HospitalEmailId, BusinessURL, HospitalAddress, CityId, RegistrationNumber, UserName, HospitalTypeID);

                if (hospitalBusinessRule.ValidationStatus == true)
                {
                    //send this password on mail
                    string Password = Utility.Utilities.RandomAlphaNumeric(6);

                    hospitalDataService.AddHospital(HospitalId, HospitalName, ContactPersonName, ContactPersonNumber, HospitalEmailId, BusinessURL, HospitalAddress, CityId, RegistrationNumber, UserName,Password,HospitalTypeID, out transaction);
                }
                else
                {
                    transaction.ReturnStatus = hospitalBusinessRule.ValidationStatus;
                    transaction.ReturnMessage = hospitalBusinessRule.ValidationMessage;
                    transaction.ValidationErrors = hospitalBusinessRule.ValidationErrors;
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
                hospitalDataService.CloseSession();
            }
        }
    }
}

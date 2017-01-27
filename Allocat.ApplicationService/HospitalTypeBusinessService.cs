using Allocat.DataModel;
using Allocat.DataServiceInterface;
using System;
using System.Collections.Generic;

namespace Allocat.ApplicationService
{
    public class HospitalTypeBusinessService
    {
        private IHospitalTypeDataService hospitalTypeDataService;

        public HospitalTypeBusinessService(IHospitalTypeDataService _hospitalTypeDataService)
        {
            hospitalTypeDataService = _hospitalTypeDataService;
        }

        public IEnumerable<HospitalType> GetHospitalType(out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            IEnumerable<HospitalType> lstHospitalType = null;
            HospitalTypeBusinessRule hospitalTypeBusinessRule = new HospitalTypeBusinessRule();
            try
            {
                hospitalTypeDataService.CreateSession();

                lstHospitalType = hospitalTypeDataService.GetHospitalType(out transaction);

                transaction.ReturnStatus = hospitalTypeBusinessRule.ValidationStatus;
                transaction.ReturnMessage = hospitalTypeBusinessRule.ValidationMessage;
                transaction.ValidationErrors = hospitalTypeBusinessRule.ValidationErrors;
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
                hospitalTypeDataService.CloseSession();
            }

            return lstHospitalType;
        }
    }
}

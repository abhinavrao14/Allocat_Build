using Allocat.DataModel;
using Allocat.DataServiceInterface;
using System;
using System.Collections.Generic;

namespace Allocat.ApplicationService
{
    public class CityBusinessService
    {
        private ICityDataService cityDataService;

        public CityBusinessService(ICityDataService _cityDataService)
        {
            cityDataService = _cityDataService;
        }

        public IEnumerable<City> GetCity(int StateId,out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            IEnumerable<City> lstCity = null;
            CityBusinessRule cityBusinessRule = new CityBusinessRule();
            try
            {
                cityDataService.CreateSession();

                cityBusinessRule.validateCityRequest(StateId);

                if (cityBusinessRule.ValidationStatus == true)
                {

                    lstCity = cityDataService.GetCitiesByStateId(StateId, out transaction);
                }
                else
                {
                    transaction.ReturnStatus = cityBusinessRule.ValidationStatus;
                    transaction.ReturnMessage = cityBusinessRule.ValidationMessage;
                    transaction.ValidationErrors = cityBusinessRule.ValidationErrors;
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
                cityDataService.CloseSession();
            }

            return lstCity;
        }
        public City GetCityById(int CityId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            City city = null;
            CityBusinessRule cityBusinessRule = new CityBusinessRule();
            try
            {
                cityDataService.CreateSession();

                cityBusinessRule.validateCityRequestByCityId(CityId);

                if (cityBusinessRule.ValidationStatus == true)
                {

                    city = cityDataService.GetCityById(CityId, out transaction);
                }
                else
                {
                    transaction.ReturnStatus = cityBusinessRule.ValidationStatus;
                    transaction.ReturnMessage = cityBusinessRule.ValidationMessage;
                    transaction.ValidationErrors = cityBusinessRule.ValidationErrors;
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
                cityDataService.CloseSession();
            }

            return city;
        }
    }
}

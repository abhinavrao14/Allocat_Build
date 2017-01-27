using Allocat.DataModel;
using Allocat.DataServiceInterface;
using System;
using System.Collections.Generic;

namespace Allocat.ApplicationService
{
    public class StateBusinessService
    {
        private IStateDataService stateDataService;

        public StateBusinessService(IStateDataService _stateDataService)
        {
            stateDataService = _stateDataService;
        }

        public IEnumerable<State> GetState(out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            IEnumerable<State> lstState = null;
            StateBusinessRule stateBusinessRule = new StateBusinessRule();
            try
            {
                stateDataService.CreateSession();

                lstState = stateDataService.GetState(out transaction);

                transaction.ReturnStatus = stateBusinessRule.ValidationStatus;
                transaction.ReturnMessage = stateBusinessRule.ValidationMessage;
                transaction.ValidationErrors = stateBusinessRule.ValidationErrors;
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
                stateDataService.CloseSession();
            }

            return lstState;
        }

        public State GetStateById(int StateId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            State state = null;
            StateBusinessRule stateBusinessRule = new StateBusinessRule();
            try
            {
                stateDataService.CreateSession();

                state = stateDataService.GetStateById(StateId, out transaction);
                transaction.ReturnStatus = stateBusinessRule.ValidationStatus;
                transaction.ReturnMessage = stateBusinessRule.ValidationMessage;
                transaction.ValidationErrors = stateBusinessRule.ValidationErrors;
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
                stateDataService.CloseSession();
            }

            return state;
        }
    }
}

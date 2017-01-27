using Allocat.DataModel;
using Allocat.DataServiceInterface;
using System;
using System.Collections.Generic;

namespace Allocat.ApplicationService
{
    public class KeyValueBusinessService
    {
        private IKeyValueDataService keyValueDataService;

        public KeyValueBusinessService(IKeyValueDataService _keyValueDataService)
        {
            keyValueDataService = _keyValueDataService;
        }

        public IEnumerable<KeyValue> Get(string Type,out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            IEnumerable<KeyValue> lst = null;
            KeyValueBusinessRule keyValueBusinessRule = new KeyValueBusinessRule();
            try
            {
                keyValueDataService.CreateSession();
                keyValueBusinessRule.ValidateType(Type);

                if (keyValueBusinessRule.ValidationStatus == true)
                {
                    lst = keyValueDataService.Get(Type, out transaction);
                }
                else
                {
                    transaction.ValidationErrors = keyValueBusinessRule.ValidationErrors;
                }

                transaction.ReturnStatus = keyValueBusinessRule.ValidationStatus;
                transaction.ReturnMessage = keyValueBusinessRule.ValidationMessage;
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
                keyValueDataService.CloseSession();
            }

            return lst;
        }
    }
}

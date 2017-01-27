using Allocat.DataServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using Allocat.DataModel;
using System.Data;
using System.Data.SqlClient;

namespace Allocat.DataService
{
    public class StateDataService : EntityFrameworkDataService, IStateDataService
    {
        public IEnumerable<State> GetState(out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            dbConnection.Configuration.ProxyCreationEnabled = false;
            IEnumerable<State> lstState = (from s in dbConnection.State
                                           select s).ToList();

            IList<State> listState = lstState.ToList();

            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add(listState.Count.ToString()+" states found.");

            return lstState;
        }

        public State GetStateById(int StateId,out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            dbConnection.Configuration.ProxyCreationEnabled = false;
            State state = (from s in dbConnection.State
                                           where s.StateId==StateId
                                           select s).FirstOrDefault();

            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add("state found.");

            return state;
        }
    }
}

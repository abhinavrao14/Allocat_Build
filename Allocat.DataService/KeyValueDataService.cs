using Allocat.DataServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using Allocat.DataModel;
using System.Data;
using System.Data.SqlClient;

namespace Allocat.DataService
{
    public class KeyValueDataService : EntityFrameworkDataService, IKeyValueDataService
    {
        public IEnumerable<KeyValue> Get(string type, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            dbConnection.Configuration.ProxyCreationEnabled = false;

            IEnumerable<KeyValue> lst=null;
            IList<KeyValue> list=null;

            if (type == "Question")
            {
                lst = (from s in dbConnection.Question
                       select new KeyValue
                       {
                           Key = s.QuestionId,
                           Value = s.QuestionBody
                       }).ToList();

                list = lst.ToList();
            }

            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add(list.Count.ToString() + " "+type + "s found.");

            return lst;
        }
    }
}

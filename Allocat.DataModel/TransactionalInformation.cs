using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allocat.DataModel
{
    public class TransactionalInformation
    {
        public bool ReturnStatus { get; set; }
        public List<String> ReturnMessage { get; set; }
        public Hashtable ValidationErrors;
        public Boolean IsAuthenicated;

        public TransactionalInformation()
        {
            ReturnMessage = new List<String>();
            ReturnStatus = true;
            ValidationErrors = new Hashtable();
            IsAuthenicated = false;
        }
    }
}

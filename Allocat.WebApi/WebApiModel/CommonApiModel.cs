using Allocat.DataModel;
using System.Collections.Generic;

namespace Allocat.WebApi.WebApiModel
{
    public class CommonApiModel : TransactionalInformation
    {
        public IEnumerable<State> States;
        public IEnumerable<City> Cities;
        public IEnumerable<HospitalType> HospitalTypes;
        public IEnumerable<KeyValue> KeyValues;
    }
}
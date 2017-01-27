using Allocat.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allocat.WebApi.WebApiModel
{
    public class HospitalApiModel : TransactionalInformation
    {
    }

    public class Hospital_DTO
    {
        public int HospitalId { get; set; }
        public string HospitalName { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonNumber { get; set; }
        public string HospitalEmailId { get; set; }
        public string BusinessURL { get; set; }
        public string HospitalAddress { get; set; }
        public int CityId { get; set; }
        public string RegistrationNumber { get; set; }
        public string UserName { get; set; }
        public int HospitalTypeID { get; set; }
    }
}
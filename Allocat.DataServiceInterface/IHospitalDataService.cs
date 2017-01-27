using Allocat.DataModel;
using System;
using System.Collections.Generic;
using System.Data;

namespace Allocat.DataServiceInterface
{
    public interface IHospitalDataService : IDataService, IDisposable
    {
        int AddHospital(int HospitalId, string HospitalName, string ContactPersonName, string ContactPersonNumber, string HospitalEmailId, string BusinessURL, string HospitalAddress, int CityId, string RegistrationNumber, string UserName,string Password, int HospitalTypeID, out TransactionalInformation transaction);
        bool ValidateUniqueHospitalEmailId(string HospitalEmailId);
        bool ValidateUniqueContactPersonNumber(string ContactPersonNumber);
        bool ValidateUniqueAATBLicenseNumber(string AATBLicenseNumber);
        bool ValidateUniqueHospitalStateLicense(string HospitalStateLicense);
        bool ValidateUniqueUserName(string UserName);
    }
}

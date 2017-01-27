using System;
using Allocat.DataModel;
using Allocat.DataServiceInterface;

namespace Allocat.ApplicationService
{
    public class HospitalBusinessRule : ValidationRules
    {
        IHospitalDataService tbDataService;

        public HospitalBusinessRule(IHospitalDataService _tbDataService)
        {
            tbDataService = _tbDataService;
        }
        public void ValidateAdd(int HospitalId, string HospitalName, string ContactPersonName, string ContactPersonNumber, string HospitalEmailId, string BusinessURL, string HospitalAddress, int CityId, string RegistrationNumber, string UserName, int HospitalTypeID)
        {
            ValidateUniqueHospitalEmailId(HospitalEmailId);
            ValidateUniqueContactPersonNumber(ContactPersonNumber);
            ValidateUniqueHospitalStateLicense(RegistrationNumber);
            ValidateUniqueUserName(UserName);
        }

        private void ValidateUniqueUserName(string UserName)
        {
            Boolean valid = tbDataService.ValidateUniqueUserName(UserName);
            if (valid == false)
            {
                AddValidationError("UserName", "User Name : " + UserName + " already exists.");
            }
        }

        private void ValidateUniqueHospitalEmailId(string HospitalEmailId)
        {
            Boolean valid = tbDataService.ValidateUniqueHospitalEmailId(HospitalEmailId);
            if (valid == false)
            {
                AddValidationError("hospitalEmailId", "Hospital EmailId : " + HospitalEmailId + " already exists.");
            }
        }

        private void ValidateUniqueHospitalStateLicense(string HospitalStateLicense)
        {
            Boolean valid = tbDataService.ValidateUniqueHospitalStateLicense(HospitalStateLicense);
            if (valid == false)
            {
                AddValidationError("HospitalStateLicense", "Hospital StateLicense : " + HospitalStateLicense + " already exists.");
            }
        }

        private void ValidateUniqueAATBLicenseNumber(string AATBLicenseNumber)
        {
            Boolean valid = tbDataService.ValidateUniqueAATBLicenseNumber(AATBLicenseNumber);
            if (valid == false)
            {
                AddValidationError("AATBLicenseNumber", "AATB License Number : " + AATBLicenseNumber + " already exists.");
            }
        }

        private void ValidateUniqueContactPersonNumber(string ContactPersonNumber)
        {
            Boolean valid = tbDataService.ValidateUniqueContactPersonNumber(ContactPersonNumber);
            if (valid == false)
            {
                AddValidationError("ContactPersonNumber", "Contact Person Number : " + ContactPersonNumber + " already exists.");
            }
        }
    }
}
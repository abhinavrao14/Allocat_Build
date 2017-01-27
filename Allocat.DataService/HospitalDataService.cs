using Allocat.DataModel;
using Allocat.DataServiceInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Allocat.DataService
{
    public class HospitalDataService : EntityFrameworkDataService, IHospitalDataService
    {
        public IEnumerable<Hospital> GetAllHospital()
        {
            AllocatDbEntities db = new AllocatDbEntities();
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                return db.Hospital.Where(t => t.IsActive == true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public IEnumerable<sp_Hospital_GetByHospitalId_Result> GetHospitalByHospitalIdId(int HospitalId)
        //{
        //    AllocatDbEntities db = new AllocatDbEntities();
        //    try
        //    {
        //        db.Configuration.ProxyCreationEnabled = false;
        //        return db.sp_Hospital_GetByHospitalId(HospitalId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public int AddHospital(int HospitalId, string HospitalName, string ContactPersonName, string ContactPersonNumber, string HospitalEmailId, string BusinessURL, string HospitalAddress, int CityId, string RegistrationNumber, string UserName, string Password, int HospitalTypeID, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            int rowAffected = 0;

            var parameterHospitalName = new SqlParameter("@HospitalName", SqlDbType.VarChar);
            parameterHospitalName.Value = HospitalName;

            var parameterContactPersonName = new SqlParameter("@ContactPersonName", SqlDbType.NVarChar);
            parameterContactPersonName.Value = ContactPersonName;

            var parameterContactPersonNumber = new SqlParameter("@ContactPersonNumber", SqlDbType.NVarChar);
            parameterContactPersonNumber.Value = ContactPersonNumber;

            var parameterHospitalEmailId = new SqlParameter("@HospitalEmailId", SqlDbType.VarChar);
            parameterHospitalEmailId.Value = HospitalEmailId;

            var parameterBusinessURL = new SqlParameter("@BusinessURL", SqlDbType.VarChar);
            parameterBusinessURL.Value = BusinessURL;

            var parameterHospitalAddress = new SqlParameter("@HospitalAddress", SqlDbType.VarChar);
            parameterHospitalAddress.Value = HospitalAddress;

            var parameterCityId = new SqlParameter("@CityId", SqlDbType.Int);
            parameterCityId.Value = CityId;

            var parameterRegistrationNumber = new SqlParameter("@RegistrationNumber", SqlDbType.VarChar);
            parameterRegistrationNumber.Value = RegistrationNumber;

            var parameterUserName = new SqlParameter("@UserName", SqlDbType.Date);
            parameterUserName.Value = UserName;

            var parameterPassword = new SqlParameter("@Password", SqlDbType.VarChar);
            parameterPassword.Value = Password;

            rowAffected = dbConnection.Database.ExecuteSqlCommand("exec dbo.sp_Hospital_Add @HospitalName, @ContactPersonName, @ContactPersonNumber, @HospitalEmailId, @BusinessURL, @HospitalAddress, @CityId, @HospitalStateLicense, @AATBLicenseNumber, @AATBExpirationDate, @AATBAccredationDate,@UserName, @Password", parameterHospitalName, parameterContactPersonName, parameterContactPersonNumber, parameterHospitalEmailId, parameterBusinessURL, parameterHospitalAddress, parameterCityId, parameterRegistrationNumber, parameterUserName,parameterPassword);

            if (rowAffected > 0)
            {
                transaction.ReturnStatus = true;
                transaction.ReturnMessage.Add("Purchase Order is updated successfully.");
            }
            else
            {
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add("Database Error");
            }

            return rowAffected;


            //transaction = new TransactionalInformation();
            //dbConnection.Configuration.ProxyCreationEnabled = false;

            //if (tissueBank != null)
            //{
            //    dbConnection.Hospital.Add(tissueBank);
            //    int rowAffected = dbConnection.SaveChanges();

            //    var _tb = (from tb in dbConnection.Hospital
            //              orderby tb.HospitalId
            //              select tb).Take(1).FirstOrDefault();

            //    _tb.CreatedBy = _tb.HospitalId;
            //    _tb.LastModifiedBy = _tb.HospitalId;

            //    dbConnection.Entry(_tb).Property(t => t.CreatedBy).IsModified = true;
            //    dbConnection.Entry(_tb).Property(t => t.LastModifiedBy).IsModified = true;
            //    rowAffected=dbConnection.SaveChanges();

            //    if (rowAffected > 0)
            //    {
            //        transaction.ReturnStatus = true;
            //        transaction.ReturnMessage.Add("Tissue Bank is registered successfully.");
            //    }
            //    else
            //    {
            //        transaction.ReturnStatus = false;
            //        transaction.ReturnMessage.Add("Database Error");
            //    }
            //}
        }

        public bool ValidateUniqueHospitalEmailId(string HospitalEmailId)
        {
            Hospital tissueBank = dbConnection.Hospital.FirstOrDefault(c => c.HospitalEmailId == HospitalEmailId);
            if (tissueBank == null)
                return true;

            return false;
        }

        public bool ValidateUniqueContactPersonNumber(string ContactPersonNumber)
        {
            Hospital tissueBank = dbConnection.Hospital.FirstOrDefault(c => c.ContactPersonNumber == ContactPersonNumber);
            if (tissueBank == null)
                return true;

            return false;
        }

        public bool ValidateUniqueRegistrationNumber(string RegistrationNumber)
        {
            Hospital hospital = dbConnection.Hospital.FirstOrDefault(h => h.RegistrationNumber == RegistrationNumber);
            if (hospital == null)
                return true;

            return false;
        }


        public bool ValidateUniqueUserName(string UserName)
        {
            User user = dbConnection.User.FirstOrDefault(u => u.UserName == UserName);
            if (user == null)
                return true;

            return false;
        }

        public int AddHospital(int HospitalId, string HospitalName, string ContactPersonName, string ContactPersonNumber, string HospitalEmailId, string BusinessURL, string HospitalAddress, int CityId, string RegistrationNumber, string UserName, int HospitalTypeID, out TransactionalInformation transaction)
        {
            throw new NotImplementedException();
        }

        public bool ValidateUniqueAATBLicenseNumber(string AATBLicenseNumber)
        {
            throw new NotImplementedException();
        }

        public bool ValidateUniqueHospitalStateLicense(string HospitalStateLicense)
        {
            throw new NotImplementedException();
        }

        //public string UpdateTb(Hospital tissueBank)
        //{
        //    using (AllocatDbEntities db = new AllocatDbEntities())
        //    {
        //        try
        //        {
        //            db.Entry(tissueBank).State = System.Data.Entity.EntityState.Modified;
        //            db.SaveChanges();
        //            return "OK";
        //        }
        //        catch (Exception ee)
        //        {
        //            return ee.Message;
        //        }
        //    }
        //}

        //public string DeleteHospital(int HospitalId)
        //{
        //    try
        //    {
        //        int _TbId = HospitalId;
        //        using (AllocatDbEntities db = new AllocatDbEntities())
        //        {
        //            var tissueBank = db.Hospital.Find(_TbId);

        //            tissueBank.IsActive = false;
        //            db.Entry(tissueBank).Property(t => t.IsActive).IsModified = true;
        //            db.SaveChanges();
        //            return "OK";
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        return ee.Message;
        //    }
        //}
    }
}

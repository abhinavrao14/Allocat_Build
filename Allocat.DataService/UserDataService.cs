using Allocat.DataServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using Allocat.DataModel;
using System.Data;
using System.Data.SqlClient;

namespace Allocat.DataService
{
    public class UserDataService : EntityFrameworkDataService, IUserDataService
    {
        public IEnumerable<TissueBankRoles_TissueBank> GetTissueBankRoles(string type, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            IEnumerable<TissueBankRoles_TissueBank> lstTissueBankRoles = (from r in dbConnection.Role
                                                                          where r.RoleName.Contains(type)
                                                                          select new TissueBankRoles_TissueBank
                                                                          {
                                                                              RoleId = r.RoleID,
                                                                              RoleName = r.RoleName
                                                                          });

            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add("Tissue Bank Roles found.");

            return lstTissueBankRoles;

        }

        public IEnumerable<sp_UserMngmt_TissueBank_GetByTissueBankId_Result> GetUser(int TissueBankId, string SearchBy, int CurrentPage, int PageSize, string SortDirection, string SortExpression, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            IEnumerable<sp_UserMngmt_TissueBank_GetByTissueBankId_Result> lstUser = dbConnection.sp_UserMngmt_TissueBank_GetByTissueBankId(TissueBankId, SearchBy, CurrentPage, PageSize, SortDirection, SortExpression);

            var numberOfRows = dbConnection.sp_UserMngmt_TissueBank_GetCountByTissueBankId(TissueBankId, SearchBy).FirstOrDefault();

            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add(numberOfRows.Value.ToString() + " Users found.");

            return lstUser;
        }

        public IEnumerable<sp_UserMngmt_TissueBank_GetByUserId_Result> GetUserDetail(int UserId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            IEnumerable<sp_UserMngmt_TissueBank_GetByUserId_Result> lstUserDetail = dbConnection.sp_UserMngmt_TissueBank_GetByUserId(UserId);

            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add("User Detail found.");

            return lstUserDetail;
        }

        public IEnumerable<sp_UserMngmt_GetUserRoleByUserId_Result> GetUserRoleByUserId(int UserId, out TransactionalInformation transaction)
        {

            transaction = new TransactionalInformation();

            IEnumerable<sp_UserMngmt_GetUserRoleByUserId_Result> lstUserRole = dbConnection.sp_UserMngmt_GetUserRoleByUserId(UserId);

            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add("User Roles found.");

            return lstUserRole;
        }

        public int User_CreateUpdateDelete(int UserId, string UserName, string Password, string FullName, string MobileNumber, string EmailId, int CreatedBy, int LastModifiedBy, int InfoId, string OperationType, bool AllowLogin, DataTable TempUser_CUD, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            int rowAffected = 0;
            var parameterUserId = new SqlParameter("@UserId", SqlDbType.Int);
            parameterUserId.Value = UserId;

            var parameterUserName = new SqlParameter("@UserName", SqlDbType.NVarChar);
            if (UserName != null)
                parameterUserName.Value = UserName;
            else
                parameterUserName.Value = DBNull.Value;

            var parameterPassword = new SqlParameter("@Password", SqlDbType.NVarChar);
            if (Password != null)
                parameterPassword.Value = Password;
            else
                parameterPassword.Value = DBNull.Value;

            var parameterFullName = new SqlParameter("@FullName", SqlDbType.NVarChar);
            if (FullName != null)
                parameterFullName.Value = FullName;
            else
                parameterFullName.Value = DBNull.Value;


            var parameterMobileNumber = new SqlParameter("@MobileNumber", SqlDbType.NVarChar);
            if (MobileNumber != null)
                parameterMobileNumber.Value = MobileNumber;
            else
                parameterMobileNumber.Value = DBNull.Value;

            var parameterEmailId = new SqlParameter("@EmailId", SqlDbType.NVarChar);
            if (EmailId != null)
                parameterEmailId.Value = EmailId;
            else
                parameterEmailId.Value = DBNull.Value;

            var parameterCreatedBy = new SqlParameter("@CreatedBy", SqlDbType.Int);
            parameterCreatedBy.Value = CreatedBy;

            var parameterLastModifiedBy = new SqlParameter("@LastModifiedBy", SqlDbType.Int);
            parameterLastModifiedBy.Value = LastModifiedBy;

            var parameterInfoId = new SqlParameter("@InfoId", SqlDbType.Int);
            parameterInfoId.Value = InfoId;

            var parameterAllowLogin = new SqlParameter("@AllowLogin", SqlDbType.Int);
            parameterAllowLogin.Value = AllowLogin;

            var parameterOperationType = new SqlParameter("@OperationType", SqlDbType.NVarChar);
            parameterOperationType.Value = OperationType;

            var parameterTempUser_CUD = new SqlParameter("@TempUser_CUD", SqlDbType.Structured);
            parameterTempUser_CUD.TypeName = "dbo.TempUser_CUD";

            

            rowAffected = dbConnection.Database.ExecuteSqlCommand("exec dbo.sp_UserMngmt_TissueBank_CreateUpdateDelete @UserId, @UserName, @Password, @FullName,  @MobileNumber, @EmailId,@CreatedBy, @LastModifiedBy,@InfoId,@AllowLogin, @OperationType, @TempUser_CUD", parameterUserId, parameterUserName, parameterPassword, parameterFullName, parameterMobileNumber, parameterEmailId, parameterCreatedBy, parameterLastModifiedBy, parameterInfoId, parameterAllowLogin, parameterOperationType, parameterTempUser_CUD);

            if (rowAffected > 0)
            {
                transaction.ReturnStatus = true;
                transaction.ReturnMessage.Add("Operation is executed successfully.");
            }
            else
            {
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add("Database Error");
            }

            return rowAffected;
        }

        public bool ValidateUniqueEmailId(string EmailId)
        {
            User user = dbConnection.User.FirstOrDefault(u => u.EmailId == EmailId);
            if (user == null)
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

    }
}

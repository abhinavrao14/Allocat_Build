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

        public int User_CreateUpdateDelete(int UserId, string UserName, string Password, string FullName, string MobileNumber, string EmailId, int CreatedBy, int LastModifiedBy, int InfoId, string OperationType, bool AllowLogin, DataTable TempUser_CUD, string PasswordQuestion, string PasswordAnswer, string SecurityQuestion, string SecurityAnswer, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            int EffectedUserId = 0, rowAffected = 0;
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
            parameterTempUser_CUD.Value = TempUser_CUD;

            var parameterPasswordQuestion = new SqlParameter("@PasswordQuestion", SqlDbType.NVarChar);
            if (PasswordQuestion != null)
                parameterPasswordQuestion.Value = PasswordQuestion;
            else
                parameterPasswordQuestion.Value = DBNull.Value;


            var parameterPasswordAnswer = new SqlParameter("@PasswordAnswer", SqlDbType.NVarChar);
            if (PasswordAnswer != null)
                parameterPasswordAnswer.Value = PasswordAnswer;
            else
                parameterPasswordAnswer.Value = DBNull.Value;


            var parameterSecurityQuestion = new SqlParameter("@SecurityQuestion", SqlDbType.NVarChar);
            if (SecurityQuestion != null)
                parameterSecurityQuestion.Value = SecurityQuestion;
            else
                parameterSecurityQuestion.Value = DBNull.Value;

            var parameterSecurityAnswer = new SqlParameter("@SecurityAnswer", SqlDbType.NVarChar);
            if (SecurityAnswer != null)
                parameterSecurityAnswer.Value = SecurityAnswer;
            else
                parameterSecurityAnswer.Value = DBNull.Value;

            rowAffected = dbConnection.Database.ExecuteSqlCommand("exec dbo.sp_UserMngmt_TissueBank_CreateUpdateDelete @UserId, @UserName, @Password, @FullName,  @MobileNumber, @EmailId,@CreatedBy, @LastModifiedBy,@InfoId,@AllowLogin, @OperationType, @TempUser_CUD , @PasswordQuestion, @PasswordAnswer, @SecurityQuestion , @SecurityAnswer", parameterUserId, parameterUserName, parameterPassword, parameterFullName, parameterMobileNumber, parameterEmailId, parameterCreatedBy, parameterLastModifiedBy, parameterInfoId, parameterAllowLogin, parameterOperationType, parameterTempUser_CUD, parameterPasswordQuestion, parameterPasswordAnswer, parameterSecurityQuestion, parameterSecurityAnswer);

            if (rowAffected > 0)
            {
                //get userid
                if (OperationType == "changePass")
                {
                    EffectedUserId = UserId;
                    transaction.ReturnMessage.Add("Password is changed successfully.");
                }
                else if (OperationType == "insert")
                {
                    User user = (from u in dbConnection.User
                                 where u.UserName == UserName
                                 select u).FirstOrDefault();
                    EffectedUserId = user.UserId;

                    transaction.ReturnMessage.Add("User is added successfully.");
                }
                else if (OperationType == "delete")
                {
                    transaction.ReturnMessage.Add("User is deleted successfully.");
                }
                else if (OperationType == "UserUpdate")
                {
                    transaction.ReturnMessage.Add("Your details are added successfully.");
                }
                else if (OperationType == "update")
                {
                    transaction.ReturnMessage.Add("User is updated successfully.");
                }
                transaction.ReturnStatus = true;
            }
            else
            {
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add("Database Error");
            }

            return EffectedUserId;
        }

        public void UserEmailVerified(int UserId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            var User = dbConnection.User.Find(UserId);

            if (User != null)
            {
                User.IsEmailVerified = true;
                dbConnection.Entry(User).Property(t => t.IsEmailVerified).IsModified = true;
                dbConnection.SaveChanges();

                transaction.ReturnStatus = true;
                transaction.ReturnMessage.Add("User Email Verified.");
            }
            else
            {
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add("UseriId not found");
            }
        }

        public User GetUserById(int UserId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            User user = (from u in dbConnection.User
                         where u.UserId == UserId
                         select u).FirstOrDefault();

            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add("User found.");

            return user;

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

        public bool ValidateSingleEmailId(string EmailId, int UserId)
        {
            User user = dbConnection.User.FirstOrDefault(c => c.EmailId == EmailId && c.UserId != UserId);
            if (user == null)
                return true;

            return false;
        }

        public bool ValidateSingleUserName(string UserName, int UserId)
        {
            User user = dbConnection.User.FirstOrDefault(c => c.UserName == UserName && c.UserId != UserId);
            if (user == null)
                return true;

            return false;
        }

        public bool ValidateUserDetailRequest(int UserId, int TissueBankId)
        {
            int count = (from u in dbConnection.User
                         join e in dbConnection.Entity on u.EntityID equals e.EntityId
                         join tb in dbConnection.TissueBank on e.InfoId equals tb.TissueBankId
                         where u.UserId == UserId && e.InfoId == TissueBankId
                         select u).Count();

            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsUserInfoAdmin(string InfoType, int UserId, int InfoId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            bool? IsUserInfoAdmin = dbConnection.usp_IsUserEntityAdmin(InfoType, UserId, InfoId).FirstOrDefault();

            if (IsUserInfoAdmin != null)
            {
                transaction.ReturnStatus = true;
                if (IsUserInfoAdmin==true)
                {
                    transaction.ReturnMessage.Add("User is Info Admin");
                    return true;
                }
                else
                {
                    transaction.ReturnMessage.Add("User is not Info Admin");
                    return false;
                }
            }
            else {
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add("Database error");
                return false;
            }
        }
    }
}

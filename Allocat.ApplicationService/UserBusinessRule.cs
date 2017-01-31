using Allocat.DataService;
using Allocat.DataServiceInterface;
using System;
using System.Data;

namespace Allocat.ApplicationService
{
    public class UserBusinessRule : ValidationRules
    {
        IUserDataService userDataService;

        public UserBusinessRule(IUserDataService _userDataService)
        {
            userDataService = _userDataService;
        }

        public void ValidateUserRequest(int TissueBankId, string SearchBy, int CurrentPage, int PageSize, string SortDirection, string SortExpression)
        {

        }

        public void ValidateUserDetailRequest(int UserId)
        {

        }

        public void ValidateUser_CUD(int UserId, string UserName, string Password, string FullName, string MobileNumber, string EmailId, int CreatedBy, int LastModifiedBy, int InfoId, string OperationType, bool AllowLogin, DataTable TempUser_CUD)
        {
            if (OperationType == "insert")
            {
                ValidateUniqueEmailId(EmailId);
                ValidateUniqueUserName(UserName);
            }
            else if (OperationType == "update")
            {
                ValidateSingleEmailId(EmailId, UserId);
                ValidateSingleUserName(UserName, UserId);
            }
        }

        public void ValidateUserRoleRequest(int UserId)
        {

        }

        private void ValidateUniqueEmailId(string EmailId)
        {
            Boolean valid = userDataService.ValidateUniqueEmailId(EmailId);
            if (valid == false)
            {
                AddValidationError("EmailId", "EmailId : " + EmailId + " already exists.");
            }
        }

        private void ValidateUniqueUserName(string UserName)
        {
            Boolean valid = userDataService.ValidateUniqueEmailId(UserName);
            if (valid == false)
            {
                AddValidationError("User Name", "User Name : " + UserName + " already exists.");
            }
        }

        private void ValidateSingleEmailId(string EmailId, int UserId)
        {
            Boolean valid = userDataService.ValidateSingleEmailId(EmailId, UserId);
            if (valid == false)
            {
                AddValidationError("EmailId", "EmailId : " + EmailId + " already exists.");
            }
        }

        private void ValidateSingleUserName(string UserName, int UserId)
        {
            Boolean valid = userDataService.ValidateSingleUserName(UserName, UserId);
            if (valid == false)
            {
                AddValidationError("User Name", "User Name : " + UserName + " already exists.");
            }
        }
    }
}
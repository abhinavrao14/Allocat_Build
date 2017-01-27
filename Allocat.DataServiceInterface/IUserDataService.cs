using Allocat.DataModel;
using System;
using System.Collections.Generic;
using System.Data;

namespace Allocat.DataServiceInterface
{
    public interface IUserDataService : IDataService, IDisposable
    {
        IEnumerable<sp_UserMngmt_TissueBank_GetByTissueBankId_Result> GetUser(int TissueBankId, string SearchBy, int CurrentPage, int PageSize, string SortDirection, string SortExpression, out TransactionalInformation transaction);
        IEnumerable<sp_UserMngmt_TissueBank_GetByUserId_Result> GetUserDetail(int UserId, out TransactionalInformation transaction);
        IEnumerable<sp_UserMngmt_GetUserRoleByUserId_Result> GetUserRoleByUserId(int UserId, out TransactionalInformation transaction);
        int User_CreateUpdateDelete(int UserId, string UserName, string Password, string FullName, string MobileNumber, string EmailId, int CreatedBy, int LastModifiedBy, int InfoId, string OperationType,bool AllowLogin, DataTable TempUser_CUD, out TransactionalInformation transaction);
        IEnumerable<TissueBankRoles_TissueBank> GetTissueBankRoles(string type,out TransactionalInformation transaction);

        bool ValidateUniqueEmailId(string EmailId);
        bool ValidateUniqueUserName(string UserName);
    }
}
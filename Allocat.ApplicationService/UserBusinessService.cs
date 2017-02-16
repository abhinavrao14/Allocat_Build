using Allocat.DataModel;
using Allocat.DataServiceInterface;
using Allocat.Utility;
using System;
using System.Collections.Generic;
using System.Data;

namespace Allocat.ApplicationService
{
    public class UserBusinessService
    {
        private IUserDataService userDataService;

        public UserBusinessService(IUserDataService _userDataService)
        {
            userDataService = _userDataService;
        }

        public IEnumerable<sp_UserMngmt_TissueBank_GetByTissueBankId_Result> GetUser(int TissueBankId, string SearchBy, int CurrentPage, int PageSize, string SortDirection, string SortExpression, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            UserBusinessRule userBusinessRule = new UserBusinessRule(userDataService);

            IEnumerable<sp_UserMngmt_TissueBank_GetByTissueBankId_Result> lstUser = null;

            try
            {
                userDataService.CreateSession();

                userBusinessRule.ValidateUserRequest(TissueBankId, SearchBy, CurrentPage, PageSize, SortDirection, SortExpression);

                if (userBusinessRule.ValidationStatus == true)
                {
                    lstUser = userDataService.GetUser(TissueBankId, SearchBy, CurrentPage, PageSize, SortDirection, SortExpression, out transaction);
                }
                else
                {
                    transaction.ReturnStatus = userBusinessRule.ValidationStatus;
                    transaction.ReturnMessage = userBusinessRule.ValidationMessage;
                    transaction.ValidationErrors = userBusinessRule.ValidationErrors;
                }
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                userDataService.CloseSession();
            }

            return lstUser;
        }

        public IEnumerable<sp_UserMngmt_TissueBank_GetByUserId_Result> GetUserDetail(int UserId, int InfoId, string InfoType, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            IEnumerable<sp_UserMngmt_TissueBank_GetByUserId_Result> lstUserDetail = null;
            UserBusinessRule userBusinessRule = new UserBusinessRule(userDataService);

            try
            {
                userDataService.CreateSession();

                userBusinessRule.ValidateUserDetailRequest(UserId,InfoId, InfoType);

                if (userBusinessRule.ValidationStatus == true)
                {
                    userDataService.CreateSession();
                    lstUserDetail = userDataService.GetUserDetail(UserId, out transaction);
                }
                else
                {
                    transaction.ReturnStatus = userBusinessRule.ValidationStatus;
                    transaction.ReturnMessage = userBusinessRule.ValidationMessage;
                    transaction.ValidationErrors = userBusinessRule.ValidationErrors;
                }

            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                userDataService.CloseSession();
            }

            return lstUserDetail;
        }

        public IEnumerable<sp_UserMngmt_GetUserRoleByUserId_Result> GetUserRoleByUserId(int UserId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            IEnumerable<sp_UserMngmt_GetUserRoleByUserId_Result> lstUserRole = null;
            UserBusinessRule userBusinessRule = new UserBusinessRule(userDataService);

            try
            {
                userDataService.CreateSession();

                userBusinessRule.ValidateUserRoleRequest(UserId);

                if (userBusinessRule.ValidationStatus == true)
                {
                    userDataService.CreateSession();
                    lstUserRole = userDataService.GetUserRoleByUserId(UserId, out transaction);
                }
                else
                {
                    transaction.ReturnStatus = userBusinessRule.ValidationStatus;
                    transaction.ReturnMessage = userBusinessRule.ValidationMessage;
                    transaction.ValidationErrors = userBusinessRule.ValidationErrors;
                }

            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                userDataService.CloseSession();
            }

            return lstUserRole;
        }

        public IEnumerable<TissueBankRoles_TissueBank> GetTissueBankRoles(string type, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            IEnumerable<TissueBankRoles_TissueBank> lstTissueBankRoles = null;
            UserBusinessRule userBusinessRule = new UserBusinessRule(userDataService);

            try
            {
                userDataService.CreateSession();
                lstTissueBankRoles = userDataService.GetTissueBankRoles(type, out transaction);
                transaction.ReturnStatus = userBusinessRule.ValidationStatus;
                transaction.ReturnMessage = userBusinessRule.ValidationMessage;
                transaction.ValidationErrors = userBusinessRule.ValidationErrors;

            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                userDataService.CloseSession();
            }
            return lstTissueBankRoles;
        }

        public void User_CreateUpdateDelete(int UserId, string UserName, string Password, string FullName, string MobileNumber, string EmailId, int CreatedBy, int LastModifiedBy, int InfoId, string OperationType, bool AllowLogin, DataTable TempUser_CUD, bool IsSendMail, string PasswordQuestion, string PasswordAnswer, string SecurityQuestion, string SecurityAnswer, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            UserBusinessRule userBusinessRule = new UserBusinessRule(userDataService);

            try
            {
                userDataService.CreateSession();

                userBusinessRule.ValidateUser_CUD(UserId, UserName, Password, FullName, MobileNumber, EmailId, CreatedBy, LastModifiedBy, InfoId, OperationType, AllowLogin, TempUser_CUD, IsSendMail,  PasswordQuestion,  PasswordAnswer,  SecurityQuestion,  SecurityAnswer);

                if (userBusinessRule.ValidationStatus == true)
                {
                  //  TempUser_CUD.Columns.Remove("Disable");

                    if (OperationType == "changePass" || OperationType == "UserUpdate")
                    {
                        if (TempUser_CUD == null)
                        {
                            DataTable dt = new DataTable();
                            dt.Columns.Add("RoleID", typeof(int));
                            dt.Columns.Add("UserID", typeof(int));
                            dt.Rows.Add(0, 0);
                            TempUser_CUD = dt;
                        }
                    }
                    else
                    {
                        TempUser_CUD.Columns.Remove("RoleName");
                        TempUser_CUD.Columns.Add("UserId", typeof(int));

                        if (OperationType == "insert")
                        {
                            Password = Utility.Utilities.RandomAlphaNumeric(6);
                            //BHASKAR SIR

                            for (int i = 0; i < TempUser_CUD.Rows.Count; ++i)
                                TempUser_CUD.Rows[i]["UserId"] = 0;
                        }
                        else
                        {
                            for (int i = 0; i < TempUser_CUD.Rows.Count; ++i)
                                TempUser_CUD.Rows[i]["UserId"] = UserId;
                        }
                    }
                    int EffectedUserId = userDataService.User_CreateUpdateDelete(UserId, UserName, Password, FullName, MobileNumber, EmailId, CreatedBy, LastModifiedBy, InfoId, OperationType, AllowLogin, TempUser_CUD,  PasswordQuestion,  PasswordAnswer,  SecurityQuestion,  SecurityAnswer, out transaction);

                    //send mail
                    if (OperationType == "changePass")
                    {
                        SMTPEmail email = new SMTPEmail();
                        MailBody mb = new MailBody();

                        if (IsSendMail == true)
                        {
                            TransactionalInformation tempTransction = new TransactionalInformation();
                            User user = userDataService.GetUserById(EffectedUserId,out tempTransction);
                            mb.Password = Password;
                            mb.ContactPersonEmailId = user.EmailId;
                            mb.ContactPersonName = user.FullName;
                            mb.UserId = EffectedUserId;
                            mb.MailType = "changePass";

                            email.sendMail(mb);
                        }
                    }
                    else if(OperationType=="insert")
                    {
                        SMTPEmail email = new SMTPEmail();
                        MailBody mb = new MailBody();

                        mb.Password = Password;
                        mb.ContactPersonEmailId = EmailId;
                        mb.ContactPersonName = FullName;
                        mb.UserId = EffectedUserId;
                        mb.MailType = "VerifyUserRegistration";

                        email.sendMail(mb);
                    }
                }
                else
                {
                    transaction.ReturnStatus = userBusinessRule.ValidationStatus;
                    transaction.ReturnMessage = userBusinessRule.ValidationMessage;
                    transaction.ValidationErrors = userBusinessRule.ValidationErrors;
                }

            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                userDataService.CloseSession();
            }
        }

        public void UserEmailVerified(int UserId, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            UserBusinessRule userBusinessRule = new UserBusinessRule(userDataService);

            try
            {
                userDataService.CreateSession();

                userBusinessRule.ValidateUserEmailVerified(UserId);

                if (userBusinessRule.ValidationStatus == true)
                {
                    userDataService.UserEmailVerified(UserId, out transaction);
                }
                else
                {
                    transaction.ReturnStatus = userBusinessRule.ValidationStatus;
                    transaction.ReturnMessage = userBusinessRule.ValidationMessage;
                    transaction.ValidationErrors = userBusinessRule.ValidationErrors;
                }

            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                userDataService.CloseSession();
            }
        }

        public bool IsUserInfoAdmin(int UserId,int InfoId, string InfoType, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
            UserBusinessRule userBusinessRule = new UserBusinessRule(userDataService);
            bool IsUserInfoAdmin = false;
            try
            {
                userDataService.CreateSession();

                userBusinessRule.ValidateIsUserInfoAdmin(UserId,InfoId, InfoType);

                if (userBusinessRule.ValidationStatus == true)
                {
                    IsUserInfoAdmin=userDataService.IsUserInfoAdmin(InfoType, UserId, InfoId, out transaction);
                }
                else
                {
                    transaction.ReturnStatus = userBusinessRule.ValidationStatus;
                    transaction.ReturnMessage = userBusinessRule.ValidationMessage;
                    transaction.ValidationErrors = userBusinessRule.ValidationErrors;
                }

            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                userDataService.CloseSession();
            }

            return IsUserInfoAdmin;
        }
    }
}

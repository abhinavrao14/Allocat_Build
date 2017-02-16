using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using Allocat.Web.Models;
using Allocat.Web.Security;
using Allocat.DataModel;
using Allocat.WebApi.WebApiModel;
using Allocat.ApplicationService;
using Allocat.DataServiceInterface;
using Allocat.DataService;
using System.Collections.Generic;

namespace Allocat.UserInterface.Controllers
{
    //[RouteArea("Common")]
    public class AccountController : Controller
    {
        IKeyValueDataService keyValueDataService;
        ITissueBankDataService tbDataService;
        IUserDataService userDataService;
        AllocatDbEntities Context = new AllocatDbEntities();

        public AccountController()
        {
            keyValueDataService = new KeyValueDataService();
            tbDataService = new TissueBankDataService();
            userDataService = new UserDataService();
        }

        [AllowAnonymous]
        public ActionResult Index(string Status)
        {
            ViewBag.Status = Status;
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel model, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                var user = Context.User.Where(u => (u.UserName == model.Username || u.EmailId == model.Username) && u.Password == model.Password && u.AllowLogin == true && u.IsEmailVerified == true).FirstOrDefault();

                if (user != null)
                {
                    var userIsLockedOut = Context.User.Where(u => u.IsLockedOut == true && u.UserId == user.UserId).FirstOrDefault();

                    if (userIsLockedOut == null)
                    {
                        var roles = Context.sp_User_GetRoleByUserName(user.UserName).ToArray();

                        var ExistInfo = (from u in Context.User
                                         join e in Context.Entity on u.EntityID equals e.EntityId
                                         join et in Context.EntityType on e.EntityTypeId equals et.EntityTypeId
                                         where u.UserId == user.UserId
                                         select new { e.InfoId, et.EntityTypeName }).FirstOrDefault();

                        CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                        serializeModel.UserId = user.UserId;
                        serializeModel.FullName = user.FullName;
                        serializeModel.roles = roles;

                        List<sp_User_GetEntityInfoByUserName_Result> EntityInfo = new List<sp_User_GetEntityInfoByUserName_Result>();

                        if (ExistInfo.InfoId != null)
                        {
                            EntityInfo = Context.sp_User_GetEntityInfoByUserName(user.UserName).ToList();
                        }
                        else
                        {
                            sp_User_GetEntityInfoByUserName_Result obj = new sp_User_GetEntityInfoByUserName_Result();
                            obj.InfoType = ExistInfo.EntityTypeName;
                            obj.InfoId = 0;
                            obj.InfoName = "";
                            EntityInfo.Add(obj);
                        }

                        serializeModel.InfoType = EntityInfo[0].InfoType;
                        serializeModel.InfoId = (int)EntityInfo[0].InfoId;
                        serializeModel.InfoName = EntityInfo[0].InfoName;


                        string userData = JsonConvert.SerializeObject(serializeModel);
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                                 1,
                                user.FullName,
                                 DateTime.Now,
                                 DateTime.Now.AddMinutes(1),
                                 false,
                                 userData);

                        string encTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                        Response.Cookies.Add(faCookie);

                        if (EntityInfo[0].InfoType.Equals("HOSPITAL"))
                        {
                            return RedirectToAction("Index", "Home", new { area = "Hospital" });
                        }
                        else if (EntityInfo[0].InfoType.Equals("TISSUEBANK"))
                        {
                            return RedirectToAction("Index", "Home", new { area = "TissueBank" });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home", new { area = "Admin" });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "You are locked out.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect username and/or password");
                }
            }


            return View(model);
        }

        [AllowAnonymous]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Account", new { area = "" });
        }

        [AllowAnonymous]
        public ActionResult TissueBankSignUp()
        {
            TransactionalInformation transaction = new TransactionalInformation();

            KeyValueBusinessService keyValueBusinessService = new KeyValueBusinessService(keyValueDataService);
            IEnumerable<KeyValue> KeyValues = keyValueBusinessService.Get("Question", out transaction);

            if (transaction.ReturnStatus == true)
            {
                ViewBag.SecurityQuestions = new SelectList(keyValueBusinessService.Get("Question", out transaction), "Key", "Value");
            }
            return View();
        }

        [HttpPost]
        public ActionResult TissueBankSignUp(SignUpViewModel model)
        {
            TransactionalInformation transaction = new TransactionalInformation();

            TissueBankApiModel tbApiModel = new TissueBankApiModel();
            TissueBankBusinessService tissueBankBusinessService = new TissueBankBusinessService(tbDataService);

            if (ModelState.IsValid)
            {
                tissueBankBusinessService.TissueBank_User_Registration(model.FullName, model.UserName, model.EmailId, model.SecurityQuestion, model.SecurityAnswer, out transaction);

                tbApiModel.ReturnMessage = transaction.ReturnMessage;
                tbApiModel.ReturnStatus = transaction.ReturnStatus;

                if (transaction.ReturnStatus == false)
                {
                    tbApiModel.ValidationErrors = transaction.ValidationErrors;
                    for (int i = 0; i < transaction.ReturnMessage.Count; ++i)
                    {
                        ModelState.AddModelError("", transaction.ReturnMessage[i]);
                    }

                    //filling up dropdown
                    KeyValueBusinessService keyValueBusinessService = new KeyValueBusinessService(keyValueDataService);
                    IEnumerable<KeyValue> KeyValues = keyValueBusinessService.Get("Question", out transaction);

                    if (transaction.ReturnStatus == true)
                    {
                        ViewBag.SecurityQuestions = new SelectList(keyValueBusinessService.Get("Question", out transaction), "Key", "Value");
                    }

                    return View();
                }
                else
                {
                    return RedirectToAction("TissueBankUser_SignUp_Successful", "Response", new { area = "" });
                }
            }
            else
            {
                //filling up dropdown
                KeyValueBusinessService keyValueBusinessService = new KeyValueBusinessService(keyValueDataService);
                IEnumerable<KeyValue> KeyValues = keyValueBusinessService.Get("Question", out transaction);

                ViewBag.SecurityQuestions = new SelectList(keyValueBusinessService.Get("Question", out transaction), "Key", "Value");

                return View(model);
            }
        }

        //[AllowAnonymous]
        //public ActionResult ForgetPassword()
        //{
        //    if (Session["UserId"] != null)
        //    {
        //        ViewBag.UserId = Convert.ToInt16(Session["UserId"]);
        //        return View();
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Account", new { area = "", Status = "Session is expired.Please do it again." });
        //    }
        //}

        //[HttpPost]
        //public ActionResult ForgetPassword(ForgetPasswordViewModel model)
        //{
        //    TransactionalInformation transaction = new TransactionalInformation();

        //    UserApiModel userApiModel = new UserApiModel();
        //    UserBusinessService userBusinessService = new UserBusinessService(userDataService);

        //    if (ModelState.IsValid)
        //    {
        //        userBusinessService.User_CreateUpdateDelete(0, "", "", "", "", model.EmailId, 0, 0, 0, "", false, null, false, out transaction);

        //        userApiModel.ReturnMessage = transaction.ReturnMessage;
        //        userApiModel.ReturnStatus = transaction.ReturnStatus;

        //        if (transaction.ReturnStatus == false)
        //        {
        //            userApiModel.ValidationErrors = transaction.ValidationErrors;
        //            for (int i = 0; i < transaction.ReturnMessage.Count; ++i)
        //            {
        //                ModelState.AddModelError("", transaction.ReturnMessage[i]);
        //            }
        //            return View();
        //        }
        //        else
        //        {
        //            return RedirectToAction("TissueBankUser_SignUp_Successful", "Response", new { area = "" });
        //        }
        //    }
        //    else
        //    {
        //        //filling up dropdown
        //        KeyValueBusinessService keyValueBusinessService = new KeyValueBusinessService(keyValueDataService);
        //        IEnumerable<KeyValue> KeyValues = keyValueBusinessService.Get("Question", out transaction);

        //        ViewBag.SecurityQuestions = new SelectList(keyValueBusinessService.Get("Question", out transaction), "Key", "Value");

        //        return View(model);
        //    }
        //}
    }
}
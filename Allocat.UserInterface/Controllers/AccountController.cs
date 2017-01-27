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
        AllocatDbEntities Context = new AllocatDbEntities();

        public AccountController()
        {
            keyValueDataService = new KeyValueDataService();
            tbDataService = new TissueBankDataService();
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
                var user = Context.User.Where(u => u.UserName == model.Username && u.Password == model.Password && u.AllowLogin == true && u.IsEmailVerified == true).FirstOrDefault();

                if (user != null)
                {
                    var userIsLockedOut = Context.User.Where(u => u.IsLockedOut == true && u.UserId == user.UserId).FirstOrDefault();

                    if (userIsLockedOut == null)
                    {
                        var roles = Context.sp_User_GetRoleByUserName(model.Username).ToArray();

                        var lstUser = (from u in Context.User
                                       join e in Context.Entity on u.EntityID equals e.EntityId
                                       join tb in Context.TissueBank on e.InfoId equals tb.TissueBankId
                                       where u.UserId == user.UserId
                                       select u).ToList();

                        CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                        serializeModel.UserId = user.UserId;
                        serializeModel.FullName = user.FullName;
                        serializeModel.roles = roles;

                        List<sp_User_GetEntityInfoByUserName_Result> EntityInfo = new List<sp_User_GetEntityInfoByUserName_Result>();

                        if (lstUser.Count != 0)
                        {
                            EntityInfo = Context.sp_User_GetEntityInfoByUserName(model.Username).ToList();
                        }
                        else
                        {
                            sp_User_GetEntityInfoByUserName_Result obj = new sp_User_GetEntityInfoByUserName_Result();
                            obj.InfoType = "TISSUEBANK";
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
            if (ModelState.IsValid)
            {
                TransactionalInformation transaction = new TransactionalInformation();

                TissueBankApiModel tbApiModel = new TissueBankApiModel();
                TissueBankBusinessService tissueBankBusinessService = new TissueBankBusinessService(tbDataService);

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
                return View(model);
            }
        }
    }
}
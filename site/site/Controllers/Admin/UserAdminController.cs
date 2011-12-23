using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using site.Models;
using site.Classes;
using site.Models.Admin;
using site.ActionFilters;

namespace site.Controllers
{
    [SeatCheck]
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class UserAdminController : BaseController
    {


        /****************** User Admin *******************/
        #region User Admin

        public ActionResult List()
        {
            MxUser mxUser = GetMxUser();
            if (mxUser.IsInRole("USERS_View"))
            {
                var model = new UserListModel(mxUser, MxUser.GetAllUsers());
                return View(model);
            }
            else
            {
                return RedirectToAction("NoAccess", "Account", new { returnUrl = "Admin/UserAdmin" });
            }
        }

        [HttpPost]
        public JsonResult ClearUserSeats(FormCollection form)
        {
            JsonResult jr = new JsonResult();
            MxUser mxUser = GetMxUser();
            if (mxUser.IsInRole("USERS_Edit"))
            {
                try
                {
                    BLL.MxLicense.UnseatUser(form["userId"], "");
                    jr = Json(new { success = "true" });
                }
                catch (Exception ex)
                {
                    jr = Json(new { success = "false", error = ex.Message });
                }
            }
            else
            {
                throw new Exception("The currently logged in user does not have user Edit rights");
            }
            return jr;
        }

        public ActionResult EditUser(string userId)
        {
            MxUser mxUser = GetMxUser();
            if (mxUser.IsInRole("USERS_View"))
            {
                var model = new EditUserModel(new MxUser(userId), mxUser);
                return View(model);
                
            }
            else
            {
                return RedirectToAction("NoAccess", "Account", new { returnUrl = "Admin/UserAdmin" });
            }
        }

        [HttpPost]
        public JsonResult EditUser(string userId, FormCollection form)
        {
            JsonResult jr = new JsonResult();
            MembershipUser mbrUser;
            MxUser mxUser = GetMxUser();
            if (form["del"] == "true")
            {
                if (mxUser.IsInRole("USERS_Delete"))
                {
                    Membership.DeleteUser(form["userName"], true);
                }
                else
                {
                    throw new Exception("The currently logged in user does not have user delete rights");
                }
            }
            else
            {   
                if (!String.IsNullOrEmpty(form["edit"]))
                {
                    if (mxUser.IsInRole("USERS_Edit"))
                    {
                        mbrUser = Membership.GetUser(new Guid(form["userId"]));
                        if (form["changePw"] == "on")
                        {
                            string generatedPw = mbrUser.ResetPassword();
                            mbrUser.ChangePassword(generatedPw, "default");
                        }
                        if (form["userSuspended"] != "on") { mbrUser.IsApproved = true; }
                        else { mbrUser.IsApproved = false; }
                        string clt = form["changedLicType"];
                        if (bool.Parse(form["changedLicType"]))
                        {
                            BLL.MxLicense.UnseatUser(mbrUser.ProviderUserKey.ToString(), "");
                        }
                    }
                    else
                    {
                        throw new Exception("The currently logged in user does not have user edit rights");
                    }
                }
                else
                {
                    if (mxUser.IsInRole("USERS_Add"))
                    {
                        mbrUser = Membership.CreateUser(form["username"], "default", form["emailAddr"]);
                        mbrUser.IsApproved = true;

                    }
                    else
                    {
                        throw new Exception("The currently logged in user does not have user add rights");
                    }
                }

                System.Web.Profile.ProfileBase profile = System.Web.Profile.ProfileBase.Create(mbrUser.UserName);

                mbrUser.Email = form["emailAddr"];

                bool compAdmin = form["isCompanyAdmin"] == "on" ? true : false;
                bool acctAdmin = form["isAcctAdmin"] == "on" ? true : false;
                bool prodAdmin = form["isProdAdmin"] == "on" ? true : false;
                bool external = form["isExternal"] == "on" ? true : false;
                bool exempt = form["isExempt"] == "on" ? true : false;
                bool cantChangePw = form["cantChangePw"] == "on" ? true : false;
                
                profile.SetPropertyValue("firstName", form["firstName"]);
                profile.SetPropertyValue("lastName", form["lastName"]);
                profile.SetPropertyValue("initials", form["initials"]);
                profile.SetPropertyValue("CompanyAdmin", compAdmin);
                profile.SetPropertyValue("CantChangePw", cantChangePw);
                profile.SetPropertyValue("ProdAdmin", prodAdmin);
                profile.SetPropertyValue("AcctAdmin", acctAdmin);
                profile.SetPropertyValue("External", external);
                profile.SetPropertyValue("Exempt", exempt);
                profile.SetPropertyValue("PwExpireInterval", form["pwExpireInterval"]);
                profile.SetPropertyValue("MinuteLimit", form["timeoutLimit"]);
                profile.SetPropertyValue("LicenseType", form["licenseType"]);

                try
                {
                    Membership.UpdateUser(mbrUser);
                    profile.Save();
                    BLL.MxUser.UpdateUserPermissions(mbrUser.ProviderUserKey.ToString(), form["suppliers"], form["customers"], form["groups"]);
                    jr = Json(new { success = "true" });

                }
                catch (Exception exc)
                {
                    jr = Json(new { success = "false", error = exc.Message });
                }
                
                
                
            }

            return jr;
        }


        //
        // GET: /Account/Users

        public ActionResult Users()
        {
            MxUser mxUser = GetMxUser();
            if (mxUser.IsInRole("USERS_View"))
            {
                var model = new UsersModel(mxUser, ""); // no user
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Account", new { returnUrl = "Admin/UserAdmin" });
            }
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ViewUser(string userId)
        {
            MxUser mxUser = GetMxUser();
            if (mxUser.IsInRole("USERS_View"))
            {
                var model = new UsersModel(mxUser, userId.ToLower());
                return View("Users", model);
            }
            else
            {
                return RedirectToAction("Login", "Account", new { returnUrl = "Admin/UserAdmin" });
            }
        }

        [HttpPost]
        public JsonResult AddUser(FormCollection form)
        {
            return EditUser("", form);
        }
        public ActionResult AddUser()
        {
            MxUser mxUser = GetMxUser();
            if (mxUser.IsInRole("USERS_Add"))
            {
                var model = new AddUserModel(mxUser);
                return View("AddUser", model);
            }
            else
            {
                throw new Exception("The currently logged in user does not have user add rights");
            }
        }

        #endregion


        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {            
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return Resources.Account.Register.dupUsrNm;

                case MembershipCreateStatus.DuplicateEmail:
                    return Resources.Account.Register.dupEmail;

                case MembershipCreateStatus.InvalidPassword:
                    return Resources.Account.Register.invalidPw;

                case MembershipCreateStatus.InvalidEmail:
                    return Resources.Account.Register.invalidEmail;

                //case MembershipCreateStatus.InvalidAnswer:
                //    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                //case MembershipCreateStatus.InvalidQuestion:
                //    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return Resources.Account.Register.invalieUn;

                case MembershipCreateStatus.ProviderError:
                    return Resources.Account.Register.authError;

                //case MembershipCreateStatus.UserRejected:
                //    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return Resources.Account.Register.unknownErr;
            }
        }
        #endregion
    }
}

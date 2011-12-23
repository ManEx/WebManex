using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using site.Models;
using System.Web.Security;
using site.Classes;
using System.Web.Routing;

namespace site.Controllers
{
    public class AccountController : BaseController
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View();
        }
        /****************** Membership *******************/
        #region Membership
        //
        // GET: /Account/LogOn

        public ActionResult LogIn(string msg, string returnUrl)
        {
            var model = new LogInModel(msg);
            return View(model);
        }

        public ActionResult NoAccess(string returnUrl)
        {
            return View(new NoAccessModel(GetMxUser()));
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogIn(LogInModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                MembershipUser mu = Membership.GetUser(model.UserName);
                if (mu != null && (!mu.IsApproved || mu.IsLockedOut))
                {
                    ModelState.AddModelError("", Resources.Account.LogIn.suspendedUser);
                }
                else
                {

                    if (Membership.ValidateUser(model.UserName, model.Password))
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

                        MxUser mxUser = new MxUser(mu.ProviderUserKey.ToString());

                        //make sure that the license type has not been tampered with
                        if (mxUser.IsCompanyAdmin || mxUser.IsAcctAdmin || mxUser.IsProdAdmin)
                        {
                            if (mxUser.LicenseType != "full")
                            {
                                mu.IsApproved = false;
                                RedirectToAction("Account", "NoSeat");
                            }
                        }

                        //clear any inactive users for concurrency
                        BLL.MxLicense.ClearInactiveSeats();

                        int lCount = BLL.MxLicense.GetLicenseSeatCount();
                        if (lCount < 1)
                        {
                            RedirectToAction("Account", "NoSeat");
                        }

                        //check to see if there's room for a seat
                        if (BLL.MxLicense.GetActiveSeatCount(mxUser.LicenseType) <= lCount) 
                        {
                            //check to see if the user already has a seat
                            if (!BLL.MxLicense.SeatCheck(mu.ProviderUserKey.ToString(), Session.SessionID))
                            {
                                //seat the user 
                                mxUser.SeatUser(Session.SessionID, "", "", Request.ServerVariables["REMOTE_ADDR"], "");
                            }
                        }
                        else
                        {
                            RedirectToAction("Account", "NoSeat");
                        }

                        int pwInterval = 0;
                        int.TryParse(mxUser.GetProperty("PwExpireInterval"), out pwInterval);
                        if (pwInterval > 0)
                        {
                            if (mu.LastPasswordChangedDate.AddDays(pwInterval) < DateTime.Today)
                            {
                                return RedirectToAction("ChangePassword", new RouteValueDictionary(
                                    new { controller = "Account", action = "ChangePassword", option = "PwExpired", username = mu.UserName }));
                            }
                        }
                        if (model.Password == "default")
                        {
                            return RedirectToAction("ChangePassword", new RouteValueDictionary(
                                    new { controller = "Account", action = "ChangePassword", option = "DefaultPw", username = mu.UserName }));
                        }
                        //add the user model to the session 
                        Session.Add("User", mxUser);
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Status", "Orders");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", Resources.Account.LogIn.Invalid);


                        //profile
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            BLL.MxLicense.UnseatUser(GetMxUser().MbrUser.ProviderUserKey.ToString(), Session.SessionID);
            Session.Clear();


            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword(string option, string username)
        {
            var model = new ChangePasswordModel(option, username);
            return View(model);
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public JsonResult ChangePassword(FormCollection form)
        {
            JsonResult jr = new JsonResult();

            // ChangePassword will throw an exception rather
            // than return false in certain failure scenarios.
            bool changePasswordSucceeded;
            try
            {
                MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                changePasswordSucceeded = currentUser.ChangePassword(form["oldPassword"], form["newPassword"]);
                if (changePasswordSucceeded)
                {
                    currentUser.IsApproved = true;
                    Membership.UpdateUser(currentUser);

                    MxUser mxUser = new MxUser(currentUser.ProviderUserKey.ToString());
                    Session["User"] = mxUser;

                    jr = Json(new { success = "true" });
                }
                else jr = Json(new { success = "false" });
            }
            catch (Exception ex)
            {
                jr = Json(new { success = "false", error = ex.Message });
            }

            return jr;
        }


        #endregion

    }
}

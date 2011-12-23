using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using site.Models;
using site.Classes;
using System.Web.Security;

namespace site.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class BaseController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            //we can do logging here if we want
            
            string controllerName = (string) filterContext.RouteData.Values["controller"];
            string actionName = (string) filterContext.RouteData.Values["action"];
            var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);

            filterContext.ExceptionHandled = true;
            //redirect to error page
            View("Error", model).ExecuteResult(this.ControllerContext);
        }
        
        public bool UserInRole(string role)
        {
            MxUser um = (MxUser)Session["User"];
            //if (um == null)
            //{
            //    //try to get the user from the currently logged in user
            //    MembershipUser mu = Membership.GetUser();
            //    if (mu != null)
            //    {
            //        um = new MxUser(mu.ProviderUserKey.ToString());
            //        Session["User"] = um; // add it to session so we have it next time
            //    }
            //}
            if (um != null)
            {
                if (um.Roles.Contains(role)) { return true; }
            }

            return false;
        }

        public MxUser GetMxUser()
        {
            if (Session["User"] != null)
            {
                return (MxUser)Session["User"];
            }
            else
            {
                MembershipUser mu = Membership.GetUser(User.Identity.Name);
                if (mu != null) { return new MxUser(mu.ProviderUserKey.ToString()); }
            }
            return null;
        }
        
        //
        // GET: /Base/
        [HttpPost]
        public JsonResult RemoveFavorite()
        {
            JsonResult jResult = new JsonResult();

            return jResult;
        }

        [HttpPost]
        public JsonResult AddFavorite(site.Classes.UI.Module module)
        {
            JsonResult jResult = new JsonResult();

            return jResult;
        }

        public ActionResult LoginCheck()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else return View();
        }
        


        public string UnauthorizedContent()
        {
            return Resources.Global.unauthorizedContent;
        }

        

        

    }
}

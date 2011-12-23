using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using site.Models;
using site.Classes;
using System.Web.Routing;
using System.Web.Security;

namespace site.ActionFilters
{
    public class SeatCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            MxUser mxUser = (MxUser)filterContext.HttpContext.Session["User"];
            RouteValueDictionary rvd = new RouteValueDictionary();
            if (mxUser != null)
            {                
                if (!BLL.MxLicense.SeatCheck(mxUser.MbrUser.ProviderUserKey.ToString(), filterContext.HttpContext.Session.SessionID))
                {
                    //user has been kicked out and needs to login        

                    //clear session
                    filterContext.HttpContext.Session.Clear();
                    FormsAuthentication.SignOut();
                    
                    rvd.Add("msg", "noseat");
                    filterContext.Result = new RedirectToRouteResult("Login", rvd);
                }
            }
            else
            {
                //user is not logged in

                rvd.Add("msg", "session");
                filterContext.Result = new RedirectToRouteResult("Login", rvd);
            }
        }
    }
}
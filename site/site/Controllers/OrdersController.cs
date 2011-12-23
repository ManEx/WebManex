using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using site.Models;
using System.Web.Security;
using site.ActionFilters;
using site.Classes;

namespace site.Controllers
{
    [SeatCheck]
    public class OrdersController : BaseController
    {
        public ActionResult Status()
        {
            MxUser mxUser = GetMxUser();

            if (mxUser.IsInRole("CUSTOMER_View"))
            {
                //var um = new UserModel(Membership.GetUser().ProviderUserKey.ToString());
                //string companyName = BLL.Customer.CustomerName(um);                
                var model = new OrderStatus(GetMxUser());
                return View(model);
            }
            else { return RedirectToAction("NoAccess", "Account", new { returnUrl = "Orders" }); }
        }

        public ContentResult List(string status, string companyId)
        {
            MxUser mxUser = GetMxUser();
                        
            if (mxUser.IsInRole("CUSTOMER_View"))
            {
                //var um = new UserModel(Membership.GetUser().ProviderUserKey.ToString());
                var model = new OrderStatus(mxUser);
                return Content(model.GetOrders(status, companyId));
            }
            else 
            {
                return Content(UnauthorizedContent());
            }

        }

        public ContentResult DueDatesShipments(string orderId)
        {
            MxUser mxUser = GetMxUser();

            if (mxUser.IsInRole("CUSTOMER_View"))
            {
                var model = new OrderStatus(mxUser);
                return Content(model.GetDueDatesShipments(orderId));
            }
            else
            {
                return Content(UnauthorizedContent());
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using site.Classes.UI;
using site.Classes;
using site.Models;
using System.Threading;
using System.Resources;

namespace site.Controllers
{
    public class HomeController : BaseController
    {   
        public ActionResult Index(HomeModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                //Dictionary<string, string> favs = Favorites.GetFavs(Response);
                //Module module = CreateHomeModule();
                //model.Module = module;
                //model.Favorites = favs;

                //SideMenuContent smc = new SideMenuContent();
                //SideMenuSection sms = new SideMenuSection();
                //sms.Title = "Hello World";
                //Dictionary<string, string> smsL1 = new Dictionary<string, string>();
                //smsL1.Add("yo dude ima link", "link1");
                //smsL1.Add("holy cow me too", "link2");
                //sms.Links = smsL1;

                //SideMenuSection sms2 = new SideMenuSection();
                //sms2.Title = "Goodbye World";
                //Dictionary<string, string> smsL2 = new Dictionary<string, string>();
                //smsL2.Add("yo dude ima link", "link1");
                //smsL2.Add("holy cow me too", "link2");
                //sms2.Links = smsL2;

                //List<SideMenuSection> smsList = new List<SideMenuSection>();
                //smsList.Add(sms);
                //smsList.Add(sms2);
                //smc.Content = smsList;

                //model.SideMenuContent = smc;//new SideMenuContent();
                ////Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es");
                ////Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es");

                return RedirectToAction("Status", "Orders");

                //return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult About()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account" , new { returnUrl = "About" });
            }
        }

        private Module CreateHomeModule()
        {
            Module Module = new Module();
            Module.Name = Resources.Shared.Nav.homeModule;
            Module.HomePage = "homepage";
            //Module.Id = "homeMnu";
            Module.IconCssClass = "moduleIcon";
            
            
            Dictionary<string, string> links = new Dictionary<string, string>();
            links.Add("linkName1", "linkUrl1");
            links.Add("linkName2", "linkUrl2");
            Module.Links = links;
            

            return Module;
        }
    }
}

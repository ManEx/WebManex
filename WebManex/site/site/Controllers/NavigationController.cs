using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using site.Models;

namespace site.Controllers
{
    public class NavigationController : BaseController
    {
        //
        // GET: /Nav/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchDdMenu(string id)
        {
            return View("DdMenu", new site.Models.MenuItemListModel(false, site.Models.Nav.NavSearchModel.GetSearchMenu(id)));
        }

        [HttpPost]
        public ActionResult NavSearch(FormCollection form)
        {
            return View("Navigation/SearchResults", 
                new site.Models.Nav.SearchResultModel(BLL.Nav.GetSearchResults(form["searchText"], form["selectedType"], form["userId"])));
        }

    }
}

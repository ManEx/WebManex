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
    public class GroupsController : BaseController
    {              

        /****************** Group Admin *******************/
        #region Group Admin

        public ActionResult List()
        {
            MxUser mxUser = GetMxUser();
            if (mxUser.IsInRole("GROUP_View"))
            {
                var model = new GroupsModel(GetMxUser(), false);
                return View(model);
            }
            else
            {
                return RedirectToAction("NoAccess", "Account", new { returnUrl = "Admin/UserAdmin" });
            }
        }

        public ActionResult Add()
        {
            MxUser mxUser = GetMxUser();
            if (mxUser.IsInRole("GROUP_Add"))
            {
                var model = new GroupsModel(GetMxUser(), true);
                return View(model);
            }
            else
            {
                return RedirectToAction("NoAccess", "Account", new { returnUrl = "Admin/Groups" });
            }
        }

        [HttpPost]
        public JsonResult AddGroup(FormCollection form)
        {
            JsonResult jr = new JsonResult();
            MxUser mxUser = GetMxUser();
            if (mxUser.IsInRole("GROUP_Add"))
            {
                try
                {
                    BLL.Groups.Add(form["groupId"], form["groupName"], form["groupDesc"]);
                    jr = Json(new { success = "true" });
                }
                catch (Exception ex)
                {
                    jr = Json(new { success = "false", error = ex.Message });
                }
            }
            else { jr = Json(new { success = "false", error = "User does not have Add rights" }); }

            return jr;
        }

        [HttpPost]
        public JsonResult DeleteGroup(FormCollection form)
        {
            JsonResult jr = new JsonResult();
            MxUser mxUser = GetMxUser();
            if (mxUser.IsInRole("GROUP_Delete"))
            {
                try
                {
                    BLL.Groups.Delete(form["groupId"]);
                    jr = Json(new { success = "true" });
                }
                catch (Exception ex)
                {
                    jr = Json(new { success = "false", error = ex.Message });
                }
            }
            else
            {
                jr = Json(new { success = "false", error = "User does not have Delete rights" });
            }

            return jr;
        }

        public ActionResult Manage()
        {
            MxUser mxUser = GetMxUser();
            if (mxUser.IsInRole("GROUP_View"))
            {
                var model = new GroupsModel(GetMxUser(), false);
                return View(model);
            }
            else
            {
                return RedirectToAction("NoAccess", "Account", new { returnUrl = "Admin/Groups" });
            }
        }

        public ActionResult Edit(string groupId, string groupName, string add)
        {
            bool adding = bool.Parse(add);
            MxUser mxUser = GetMxUser();
            if (mxUser.IsInRole("GROUP_View") || mxUser.IsInRole("GROUP_Add") || mxUser.IsInRole("GROUP_Edit") || mxUser.IsInRole("GROUP_Delete"))
            {
                var model = new GroupsModel(GetMxUser(), groupId, groupName, adding);
                return View(model);
            }
            else
            {
                return RedirectToAction("NoAccess", "Account", new { returnUrl = "Admin/Groups" });
            }
        }

        [HttpPost]
        public JsonResult AddRolesToGroups(FormCollection form)
        {
            JsonResult jr = new JsonResult();
            MxUser mxUser = GetMxUser();

            try
            {

                if (mxUser.IsInRole("GROUP_Edit"))
                {
                    
                    BLL.Groups.UpdateRoleInGroups(form["groups"].Replace("addRole_", ""), form["role"]);
                    jr = Json(new { success = "true" });
                }
                else
                {
                    jr = Json(new { success = "false", error = "User does not have Group Edit rights" });
                }
            }
            catch (Exception ex)
            {
                jr = Json(new { success = "false", error = ex.Message });
            }
            return jr;
        }

        [HttpPost]
        public JsonResult EditGroup(FormCollection form)
        {
            JsonResult jr = new JsonResult();
            MxUser mxUser = GetMxUser();
            
            if (form["del"] == "true")
            {
                if (mxUser.IsInRole("GROUP_Delete"))
                {
                    BLL.Groups.Delete(form["groupId"]);
                }
                else
                {
                    jr = Json(new { success = "false", error = "User does not have Delete rights" });
                }
            }
            else
            {
                if (mxUser.IsInRole("GROUP_Edit"))
                {
                    if (!String.IsNullOrEmpty(form["edit"]))
                    {
                        try
                        {
                            BLL.Groups.Update(form["groupId"], form["roles"], form["groupName"], form["description"]);
                            jr = Json(new { success = "true" });
                        }
                        catch (Exception ex)
                        {
                            jr = Json(new { success = "false", error = ex.Message });
                        }

                    }
                }
                else
                {
                    jr = Json(new { success = "false", error = "User does not have Edit rights" });
                }
            }

            return jr;
        }

        #endregion

    }
}

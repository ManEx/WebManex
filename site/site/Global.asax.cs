using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace site
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            /****************** Account Routes *******************/
            #region Account Routes

            routes.MapRoute(
                "Login",
                "Account/Login",
                new { controller = "Account", action = "Login" }
            );

            routes.MapRoute(
                "NoAccess",
                "Account/NoAccess",
                new { controller = "Account", action = "NoAccess" }
            );


            #endregion

            /****************** User Admin Routes *******************/
            #region User Admin Routes
            
            routes.MapRoute(
                "Users",
                "Admin/Users",
                new { controller = "UserAdmin", action = "Users" }
            );
            routes.MapRoute(
                "ViewAddUser",
                "Admin/UserAdmin/Add",
                new { controller = "UserAdmin", action = "AddUser" }
            );

            routes.MapRoute(
                "AddUser",
                "Admin/UserAdmin/AddUser",
                new { controller = "UserAdmin", action = "AddUser" }
            );
            routes.MapRoute(
                "ListUsers",
                "Admin/UserAdmin/List/{userId}",
                new { controller = "UserAdmin", action = "List", userId = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                "ViewUserAjax",
                "Admin/UserAdmin/Edit/{userId}",
                new { controller = "UserAdmin", action = "EditUser", userId = UrlParameter.Optional }
            );

            routes.MapRoute(
                "ViewUser",
                "Admin/Users/{userId}",
                new { controller = "UserAdmin", action = "ViewUser", userId = UrlParameter.Optional }
            );

            routes.MapRoute(
                "ClearUserSeats",
                "Admin/UserAdmin/ClearUserSeats",
                new { controller = "UserAdmin", action = "ClearUserSeats", userId = UrlParameter.Optional }
            );

            routes.MapRoute(
                "EditUser",
                "Admin/UserAdmin/EditUser",
                new { controller = "UserAdmin", action = "EditUser" }
            );
            #endregion

            /****************** Group Admin routes *******************/
            #region Group Admin routes

            routes.MapRoute(
                "ListGroups",
                "Admin/Groups/List",
                new { controller = "Groups", action = "List" }
            );

            routes.MapRoute(
                "ViewGroup",
                "Admin/Groups/Edit/{groupId}",
                new { controller = "Groups", action = "Edit", groupId = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                "EditGroup",
                "Admin/Groups/EditGroup",
                new { controller = "Groups", action = "EditGroup" }
            );
            routes.MapRoute(
                "AddGroup",
                "Admin/Groups/AddGroup",
                new { controller = "Groups", action = "AddGroup" }
            );
            routes.MapRoute(
                "NewGroup",
                "Admin/Groups/Add",
                new { controller = "Groups", action = "Add" }
            );

            routes.MapRoute(
                "DeleteGroup",
                "Admin/Groups/DeleteGroup",
                new { controller = "Groups", action = "DeleteGroup" }
            );

            routes.MapRoute(
                "ManageGroups",
                "Admin/Groups",
                new { controller = "Groups", action = "Manage", userId = UrlParameter.Optional }
            );

            routes.MapRoute(
                "AddRoleToGroup",
                "Admin/Groups/AddRolesToGroups",
                new { controller = "Groups", action = "AddRolesToGroups" }
            );
            #endregion 

            /****************** Orders Routes *******************/
            #region Orders Routes

            routes.MapRoute(
                "OrderList",
                "Orders/List",
                new { controller = "Orders", action = "List" }
            );
            routes.MapRoute(
                "OrderDueDatesShipment",
                "Orders/DueDatesShipment",
                new { controller = "Orders", action = "DueDatesShipments" }
            );
            routes.MapRoute(
                "OrderStatus",
                "Orders",
                new { controller = "Orders", action = "Status" }
            );

            #endregion

            /****************** Nav Routes *******************/
            #region Nav Routes
            routes.MapRoute(
                "SearchDdMenu",
                "SearchDdMenu/{id}", // URL with parameters
                new { controller = "Navigation", action = "SearchDdMenu", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
               "NavSearch",
               "NavSearch", 
               new { controller = "Navigation", action = "NavSearch" } // Parameter defaults
           );

            #endregion

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using site.Classes;
using System.Data;
using System.Text;

namespace site.Models
{
    public class MenuItemListModel
    {
        public bool ShowSelectFirstItem { get; set; }
        public List<MenuItem> MenuItems { get; set; }

        public MenuItemListModel(bool showSelectFirstItem, List<MenuItem> menuItems)
        {
            ShowSelectFirstItem = showSelectFirstItem;
            MenuItems = menuItems;
        }
    }

    public class ButtonModel
    {
        public MxUser Usr { get; set; }
        public string Id { get; set; }
        public string[] RequiredRoles { get; set; }
        public string ButtonText { get; set; }
        public string JsCall { get; set; }
        public string ButtonDescription { get; set; }
        public bool ShowBtn { get; set; }
        public string CssClass { get; set; }
        public string IconClass { get; set; }
        public string Tooltip { get; set; }
        public bool ShowBtnText { get; set; }
        
        /// <summary>
        /// Constructor for the ButtonModel
        /// </summary>
        /// <param name="user"></param>
        /// <param name="requiredRoles">if the user has any of these roles, then the user will see the button.  Pass null or empty array if the button does not require any roles to see</param>
        /// <param name="buttonText">localized text</param>
        /// <param name="buttonJsCall">Javascript that will be executed when the button is clicked</param>
        /// <param name="buttonDescription">localized description of the button</param>
        public ButtonModel(MxUser user, string id, string[] requiredRoles, string buttonText, string buttonJsCall, string buttonDescription, 
            string cssClass, string iconClass, bool showButtonText)
        {
            Usr = user;
            Id = id;
            RequiredRoles = requiredRoles;
            ButtonText = buttonText;
            JsCall = buttonJsCall;
            ButtonDescription = buttonDescription;
            ShowBtn = false;
            if (string.IsNullOrEmpty(iconClass))
            {
                CssClass = cssClass;
            }
            else
            {
                CssClass = cssClass + " icon";
            }
            IconClass = iconClass;
            ShowBtnText = showButtonText;

            if (requiredRoles == null || requiredRoles.Length == 0) { ShowBtn = true; }
            else
            {
                for (int i = 0; i < requiredRoles.Length; i++)
                {
                    if (user.IsInRole(requiredRoles[i]))
                    {
                        ShowBtn = true;
                        break;
                    }
                }
            }

            if (!ShowBtnText)
            {
                Tooltip = buttonText;
            }
        }

        
    }

    public class RequiredRoleModel
    {
        public string ModalId { get; set; }
        public string FunctionalityName { get; set; }
        public List<RequiredRole> RequiredRoles { get; set; }

        public RequiredRoleModel(string[] roles, string functionalityName)
        {
            RequiredRoles = new List<RequiredRole>();
            ModalId = Guid.NewGuid().ToString();
            for (int i = 0; i < roles.Length; i++)
            {
                RequiredRoles.Add(new RequiredRole(roles[i]));
            }
        }
        
    }

    public class RequiredRole
    {
        public string RoleStr { get; set; }
        public string Id { get; set; }
        public DataTable RolesDt { get; set; }
        public RequiredRole(string role)
        {
            RoleStr = role;
            RolesDt = BLL.Groups.ListGroupsForRole(role);
            this.Id = Guid.NewGuid().ToString();
        }
    }

   
    public class MenuItem
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public MenuItem(string resourceNameSpace, string resourceId, string defaultTxt, string val)
        {
            Id = val;
            Text = Resources.Localizer.GetString(resourceNameSpace, resourceId, defaultTxt);
        }
    }
}
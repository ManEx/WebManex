using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;

namespace site.Models
{
    public class UserModel
    {
        #region properties
        public MembershipUser MbrUser { get; set; }
        public System.Web.Profile.ProfileBase Profile { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Initials { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public bool IsCompanyAdmin { get; set; }
        #endregion

        #region ctor
        public UserModel(string userId)
        {
            MbrUser = Membership.GetUser(new Guid(userId));
            Profile = System.Web.Profile.ProfileBase.Create(MbrUser.UserName);
            FirstName = GetProperty("FirstName");
            LastName = GetProperty("LastName");
            DisplayName = string.Format("{0} {1}", GetProperty("firstName"), GetProperty("lastName"));
            Initials = GetProperty("initials");
            Email = MbrUser.Email;
            Company = GetProperty("companyId");
            IsCompanyAdmin = Roles.IsUserInRole(MbrUser.UserName, "CompanyAdmin");
        }
        #endregion

        #region methods
        public string GetProperty(string property)
        {
            object p = Profile.GetPropertyValue(property);
            if (p != null) { return p.ToString(); }
            return "";
        }

        public static void UpdateUser(FormCollection form)
        {
            MembershipUser mbrUser;
            if (!String.IsNullOrEmpty(form["edit"]))
            {
                mbrUser = Membership.GetUser(new Guid(form["userId"]));
                if (form["changePw"] == "on")
                {
                    mbrUser.ChangePassword(mbrUser.GetPassword(), "default");
                }
            }
            else
            {
                mbrUser = Membership.CreateUser(form["username"], "default", form["emailAddr"]);
            }

            System.Web.Profile.ProfileBase profile = System.Web.Profile.ProfileBase.Create(mbrUser.UserName);
            string x = form["emailAddr"];
            string y = form["initials"];
            mbrUser.Email = form["emailAddr"];
            
            
            if (form["isAdmin"] == "on" && !Roles.IsUserInRole(mbrUser.UserName, "CompanyAdmin"))
            {
                Roles.AddUserToRole(mbrUser.UserName, "CompanyAdmin");
            }
            else if (form["isAdmin"] != "on" && Roles.IsUserInRole(mbrUser.UserName, "CompanyAdmin"))
            {
                Roles.RemoveUserFromRole(mbrUser.UserName, "CompanyAdmin");
            }
            profile.SetPropertyValue("firstName", form["firstName"]);
            profile.SetPropertyValue("lastName", form["lastName"]);
            profile.SetPropertyValue("initials", form["initials"]);
            Membership.UpdateUser(mbrUser);
            profile.Save();
        }

        public static List<UserModel> GetAllUsers()
        {
            List<UserModel> users = new List<UserModel>();
            foreach (MembershipUser u in Membership.GetAllUsers())
            {
                users.Add(new UserModel(u.ProviderUserKey.ToString()));
            }
            return users;
        }
        #endregion
    }
}
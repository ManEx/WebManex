using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using System.Data;
using System.Configuration;

namespace site.Classes
{
    public class MxUser
    {
        #region properties
        public MembershipUser MbrUser { get; set; }
        public System.Web.Profile.ProfileBase Profile { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Initials { get; set; }
        public string Email { get; set; }
        public string LicenseType { get; set; }
        public DataTable Customers { get; set; }
        public DataTable Suppliers { get; set; }
        public DataTable Groups { get; set; }
        public bool IsCompanyAdmin { get; set; }
        public bool IsAcctAdmin { get; set; }
        public bool IsProdAdmin { get; set; }
        public bool IsExempt { get; set; }
        public bool IsExternal { get; set; }
        public bool CantChangePw { get; set; }
        public DateTime LastPwChangeDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string PwExpireInterval { get; set; }
        public string TimeoutLimit { get; set; }
        public List<string> Roles { get; set; }
        public bool ShowClearSeats { get; set; }
       
        #endregion

        #region ctor
        public MxUser(string userId)
        {
            MbrUser = Membership.GetUser(new Guid(userId));
            Profile = System.Web.Profile.ProfileBase.Create(MbrUser.UserName);
            FirstName = GetProperty("FirstName");
            LastName = GetProperty("LastName");
            LicenseType = GetProperty("LicenseType");
            DisplayName = string.Format("{0} {1}", GetProperty("firstName"), GetProperty("lastName"));
            Initials = GetProperty("initials");
            Email = MbrUser.Email;
            IsCompanyAdmin = GetBoolProperty("CompanyAdmin");
            IsAcctAdmin = GetBoolProperty("AcctAdmin");
            IsProdAdmin = GetBoolProperty("ProdAdmin");
            IsExempt = GetBoolProperty("Exempt");
            IsExternal = GetBoolProperty("External");
            LastPwChangeDate = MbrUser.LastPasswordChangedDate;
            LastLoginDate = MbrUser.LastLoginDate;
            PwExpireInterval = GetProperty("PwExpireInterval");
            TimeoutLimit = GetProperty("MinuteLimit");
            CantChangePw = GetBoolProperty("CantChangePw");
            
            DataSet userInfoDs = BLL.MxUser.GetUserInfo(userId);
            Customers = userInfoDs.Tables[1];
            Suppliers = userInfoDs.Tables[2];
            Groups = userInfoDs.Tables[3];
            Roles = BLL.MxUser.AssignedRoles(userId, LicenseType);
            Roles.Sort();

            if (userInfoDs.Tables[0] != null  && userInfoDs.Tables[0].Rows.Count > 0)
            {
                int activeSeats = (int)userInfoDs.Tables[0].Rows[0]["activeSeats"];
                ShowClearSeats = activeSeats > 0;
            }

        }

        /// <summary>
        /// Default Constructor
        /// Use this only for adding new users
        /// </summary>
        public MxUser()
        {
            DataSet userInfoDs = BLL.MxUser.GetUserInfo(Guid.NewGuid().ToString());
            Customers = userInfoDs.Tables[1];
            Suppliers = userInfoDs.Tables[2];
            Groups = userInfoDs.Tables[3];
            PwExpireInterval = ConfigurationSettings.AppSettings["PasswordExpireInterval"];
        }
        #endregion

        #region methods
        public string GetProperty(string property)
        {
            object p = Profile.GetPropertyValue(property);
            if (p != null) { return p.ToString(); }
            return "";
        }
        private bool GetBoolProperty(string property)
        {
            object p = Profile.GetPropertyValue(property);
            if (p != null) { return (bool)p; }
            return false;
        }

        public bool IsInRole(string role)
        {
            if (Roles.Contains(role) || IsCompanyAdmin) { return true; }
            return false;
        }

        #region static methods

        /// <summary>
        /// GetAllUsers
        /// </summary>
        /// <returns>List of all MxUsers in the system.  </returns>
        public static DataTable GetAllUsers()
        {
            return BLL.MxUser.GetAll();
        }


        public void SeatUser(string sessionId, string workStationId, string lastModule, string ipAddress, 
            string licenceTypeRoles)
        {
            BLL.MxLicense.SeatUser(this.MbrUser.ProviderUserKey.ToString(), sessionId, workStationId, lastModule, ipAddress);
        }

        


        #endregion

        #endregion
    }
}
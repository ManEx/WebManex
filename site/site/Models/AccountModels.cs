using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using site.Classes;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;

namespace site.Models
{
    public class AccountModels
    {
    }

    public class NoAccessModel
    {
        public MxUser Usr { get; set; }

        public NoAccessModel(MxUser user)
        {
            Usr = user;
        }
    }

    public class ChangePasswordModel
    {
        public bool PwExpired { get; set; }
        public bool DefaultPw { get; set; }
        public string RootUrl { get { return string.Format("http://{0}", HttpContext.Current.Request.Url.Authority); } }
        public ChangePasswordModel(string option, string username)
        {
            MembershipUser mu = Membership.GetUser(username);
            switch (option)
            {
                case "PwExpired":
                    PwExpired = true;
                    mu.IsApproved = false;
                    Membership.UpdateUser(mu);
                    break;
                case "DefaultPw":
                    DefaultPw = true;
                    mu.IsApproved = false;
                    Membership.UpdateUser(mu);
                    break;
            }
        }
    }



    public class LogInModel
    {
        public bool ShowSessionExpired { get; set; }
        public bool ShowNoSeat { get; set; }
        public LogInModel(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                switch (msg)
                {
                    case "noseat":
                        ShowNoSeat = true;
                        break;
                    case "session":
                        ShowSessionExpired = true;
                        break;
                }
            }
        }
        public LogInModel() { }
        [Required]
        //[Display(Name = Resources.Account.LogIn.Username)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        //[Display(Name = Resources.Account.LogIn.Password)]
        public string Password { get; set; }

        //[Display(Name = Resources.Account.LogIn.Remember)]
        public bool RememberMe { get; set; }

    }
}
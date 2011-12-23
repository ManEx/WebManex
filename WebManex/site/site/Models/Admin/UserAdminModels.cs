using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using System.Resources;
using System.Web;
using site.Classes;
using System.Data;

namespace site.Models.Admin
{
    /// <summary>
    /// Model for edit user page
    /// </summary>
    public class EditUserModel
    {
        public MxUser EditUser { get; set; }
        public MxUser Usr { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="editUser">User that is being edited</param>
        /// <param name="user">Logged in user</param>
        public EditUserModel(MxUser editUser, MxUser user)
        {
            EditUser = editUser;
            Usr = user;
        }
    }

    /// <summary>
    /// Model for adding a new user
    /// </summary>
    public class AddUserModel
    {
        public MxUser Usr { get; set; }
        public MxUser AddUser { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user">Logged in user </param>
        public AddUserModel(MxUser user)
        {
            Usr = user;
            //create a default (blank user)
            AddUser = new MxUser();
        }
    }
    
    /// <summary>
    /// Model for the user list (left side)
    /// </summary>
    public class UserListModel
    {
        public MxUser Usr { get; set; }
        public DataTable Users { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUsr">logged in user</param>
        /// <param name="userDt">DataTable of all users in the system</param>
        public UserListModel(MxUser currentUsr, DataTable userDt)
        {
            Usr = currentUsr;
            Users = userDt;
        }
    }

    /// <summary>
    /// Model for the main user admin page
    /// </summary>
    public class UsersModel
    {
        public MxUser Usr { get; set; }
        public string UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user">logged in user</param>
        /// <param name="userId">userid - if we are trying to see a particular user</param>
        public UsersModel(MxUser user, string userId)
        {
            Usr = user;
            UserId = userId;
        }       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.Security;
using System.Configuration.Provider;

namespace BLL
{
    public class MxUser
    {

        #region properties

        public string Name { get; private set; }


        #endregion

        #region constructors

        public MxUser()
        {
        }

        #endregion

        #region methods

        #region User Management

        public static DataSet GetUserInfo(string userId)
        {           
            return DAL.MxUser.GetUserInfo(userId);
        }

        public static void UpdateUserPermissions(string userId, string suppliers, string customers, string groups)
        {
            DAL.MxUser.UpdateUserPermissions(userId, suppliers, customers, groups);
        }

        public static List<string> AssignedRoles(string userId, string licenseType)
        {
            List<string> roles = new List<string>();

            DataTable rolesDt = DAL.MxUser.GetAssignedRoles(userId, licenseType);
            foreach (DataRow roleRow in rolesDt.Rows)
            {
                roles.Add(roleRow["RoleName"].ToString());
            }

            return roles;
        }

        public static DataTable GetAll()
        {
            return DAL.MxUser.GetAll();
        }

        #endregion

        #endregion
    }
        
    
}

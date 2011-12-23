using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class MxUser
    {

        #region User Rights
        public static DataSet GetUserInfo(string userId)
        {
            SqlParameter[] sqlParams = 
            { 
                new SqlParameter("@UserId", userId)
            };

            return DBHelper.GetDataSet("aspmnxSP_GetUserInfo", sqlParams);
        }

        public static DataTable GetAssignedRoles(string userId, string licenseType)
        {
            SqlParameter[] sqlParams = 
            { 
                new SqlParameter("@UserId", userId),
                new SqlParameter("@licenseTypeRoles", licenseType)
            };

            return DBHelper.GetDataTable("aspMnxSp_GetRoles4User", sqlParams);
        }

        public static void UpdateUserPermissions(string userId, string suppliers, string customers, string groups)
        {
            SqlParameter[] sqlParams = 
            { 
                new SqlParameter("@UserId", new Guid(userId)),
                new SqlParameter("@Suppliers", suppliers),
                new SqlParameter("@Customers", customers),
                new SqlParameter("@Groups", groups)
            };
            DBHelper.ExecuteNonQuery("aspmnx_UpdateUserPermissions", sqlParams);
        }
        #endregion

        #region Favorites

        private int AddFavorite(string userId, int moduleId)
        {
            SqlParameter favId = new SqlParameter("@UniqueUPFA", SqlDbType.Int);
            SqlParameter[] sqlParams = 
            { 
                new SqlParameter("@lcFk_UniqUser", userId), 
                new SqlParameter("@lnFk_UniqForms", moduleId)
            };
            return DBHelper.ExecuteNonQuery("Sp_InsertUsersPrefForms", sqlParams);
        }

        private void DeleteFavorite(int favId)
        {
            SqlParameter[] sqlParams = 
            { 
                new SqlParameter("@lcUniqueUPFA", favId)
            };
            DBHelper.ExecuteNonQuery("Sp_deleteUsersPrefForms", sqlParams);
        }

        public static Dictionary<string, string> GetUserFavorites(string userId)
        {
            Dictionary<string, string> favs = new Dictionary<string, string>();
            DataTable favDt = GetFavoritesDT(userId);
            foreach (DataRow dr in favDt.Rows)
            {

            }
            return favs;
        }

        private static DataTable GetFavoritesDT(string userId)
        {
            SqlParameter[] sqlParams = 
            { 
                new SqlParameter("@lcFk_UniqUser", userId)
            };

            return DBHelper.GetTable("sp_GetUsersPrefApss", sqlParams);
        }

        #endregion

        #region User Management

        public static void UpdateUser(Guid userId, Guid companyId, string phone, string username)
        {
            SqlParameter[] sqlParams = 
            { 
                new SqlParameter("@UserId", userId),
                new SqlParameter("@companyId", companyId),
                //new SqlParameter("@roleName", phone),
                new SqlParameter("@userName", username)
            };

            DBHelper.ExecuteNonQuery("updateUser", sqlParams);
        }


        public static DataTable GetAll()
        {
            SqlParameter[] sqlParams = null;

            return DBHelper.GetTable("aspmnxSP_GetUsers", sqlParams);
        }

        
        public static DataTable GetById(Guid id)
        {
            SqlParameter[] sqlParams = 
            { 
                new SqlParameter("@userId", id)
            };

            return DBHelper.GetTable("user_profile_get", sqlParams);
        }
        #endregion
    }
}

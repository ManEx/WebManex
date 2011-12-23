using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class UserTools
    {
        private static DataTable GetFavoritesDT(string userId)
        {
            SqlParameter[] sqlParams = 
            { 
                new SqlParameter("@lcFk_UniqUser", userId)
            };

            return DBHelper.GetTable("sp_GetUsersPrefApss", sqlParams);
        }

        private static DataTable GetModuleRights(string userId)
        {
            SqlParameter[] sqlParams = 
            { 
                new SqlParameter("@lcFk_UniqUser", userId)
            };

            return DBHelper.GetTable("sp_GetUsersPrefApss", sqlParams);
        }

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

        public static DataTable Verify(string userId, string password)
        {
            SqlParameter[] sqlParams = 
            { 
                new SqlParameter("@pcUserId", userId),
                new SqlParameter("@pcUserEntry", password)
            };

            return DBHelper.GetTable("Sp_Userverify", sqlParams);
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

        public static DataTable GetAll(Guid companyId)
        {
            SqlParameter[] sqlParams = 
            { 
                new SqlParameter("@companyId", companyId)
            };

            return DBHelper.GetTable("user_profile_getAll", sqlParams);
        }

        
        public static DataTable GetById(Guid id)
        {
            SqlParameter[] sqlParams = 
            { 
                new SqlParameter("@userId", id)
            };

            return DBHelper.GetTable("user_profile_get", sqlParams);
        }
    }
}

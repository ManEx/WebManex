using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class Groups
    {
        public static DataTable ListGroups()
        {
            SqlParameter[] sqlParams = null;

            return DBHelper.GetTable("aspmnxSP_GetGroups", sqlParams);
        }

        public static DataTable ListGroupsForRole(string role)
        {
            SqlParameter[] sqlParams = 
            { 
                new SqlParameter("@roleName", role)
            };

            return DBHelper.GetTable("aspmnxSP_GetGroups4Role", sqlParams);
        }

        public static DataTable RolesForGroup(Guid groupId)
        {
            SqlParameter[] sqlParams = 
            { 
                new SqlParameter("@GroupId", groupId)
            };

            return DBHelper.GetTable("aspmnxSP_GetRoles4Group", sqlParams);
        }

        public static void Update(string groupId, string roles, string groupName, string description)
        {
            SqlParameter[] sqlParams = 
            { 
                new SqlParameter("@Groupid", groupId),
                new SqlParameter("@GroupName", groupName),
                new SqlParameter("@GroupDescription", description),
                new SqlParameter("@Roles", roles)
            };
            
            DBHelper.ExecuteNonQuery("aspmnx_GroupUpdate", sqlParams);
        }

        public static void Add(string groupId, string groupName, string groupDesc)
        {
            SqlParameter[] sqlParams = 
            { 
                new SqlParameter("@Groupid", groupId),
                new SqlParameter("@GroupName", groupName),
                new SqlParameter("@GroupDescription", groupDesc)
            };

            DBHelper.ExecuteNonQuery("aspmnx_GroupAdd", sqlParams);
        }

        public static void UpdateRoleInGroups(string groupsStr, string roleName)
        {
            SqlParameter[] sqlParams = 
            { 
                new SqlParameter("@Groups", groupsStr),
                new SqlParameter("@roleName", roleName)
            };

            DBHelper.ExecuteNonQuery("aspmnx_addRoleToGroups", sqlParams);
        }

        public static void Delete(string groupId)
        {
            SqlParameter[] sqlParams = 
            { 
                new SqlParameter("@Groupid", groupId)
            };

            DBHelper.ExecuteNonQuery("aspmnx_GroupDelete", sqlParams);
        }
    }
}

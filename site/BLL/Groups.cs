using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BLL
{
    public class Groups
    {
        public static DataTable ListGroups()
        {
            return DAL.Groups.ListGroups();
        }
        public static DataTable ListGroupsForRole(string role)
        {
            return DAL.Groups.ListGroupsForRole(role);
        }
        public static Dictionary<string, string> GetGroupsDict()
        {
            Dictionary<string, string> groups = new Dictionary<string, string>();

            DataTable gDt = DAL.Groups.ListGroups();
            foreach (DataRow gRow in gDt.Rows)
            {
                if (!groups.ContainsKey(gRow["groupName"].ToString()))
                {
                    groups.Add(gRow["groupName"].ToString(), gRow["groupId"].ToString());
                }
            }

            return groups;
        }

        public static DataTable RolesForGroup(Guid groupId)
        {
            return DAL.Groups.RolesForGroup(groupId);
        }

        public static List<GroupRole> RolesForGroupList(Guid groupId)
        {
            List<GroupRole> groupRoles = new List<GroupRole>();
            DataTable dt = DAL.Groups.RolesForGroup(groupId);
            DataView dv = dt.DefaultView;
            dv.Sort = "RoleName";
            foreach (DataRowView row in dv)
            {
                groupRoles.Add(new GroupRole(new Guid(row["RoleId"].ToString()), row["RoleName"].ToString(),
                    row["Description"].ToString(), (bool)row["Assigned"]));
            }
            return groupRoles;
        }

        public static void Update(string groupId, string roles, string groupName, string description)
        {
            DAL.Groups.Update(groupId, roles, groupName, description);
        }
        public static void Add(string groupId, string groupName, string groupDesc)
        {
            DAL.Groups.Add(groupId, groupName, groupDesc);
        }

        public static void Delete(string groupId)
        {
            DAL.Groups.Delete(groupId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupsStr">comma delimited string of group ID's that will have this role (if a group is omitted, they will no longer have this role)</param>
        /// <param name="roleName">unique role name (eg USERS_Add)</param>
        public static void UpdateRoleInGroups(string groupsStr, string roleName)
        {
            DAL.Groups.UpdateRoleInGroups(groupsStr, roleName);
        }
    }

    public class GroupRole
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool Assigned { get; set; }

        public GroupRole(Guid roleId, string roleName, string description, bool assigned)
        {
            RoleId = roleId;
            RoleName = roleName;
            Description = description;
            Assigned = assigned;
        }
        public GroupRole() { }
    }
}

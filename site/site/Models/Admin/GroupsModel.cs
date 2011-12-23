using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using site.Classes;

namespace site.Models.Admin
{
    /// <summary>
    /// Model for the Groups pages
    /// </summary>
    public class GroupsModel
    {
        //Contains all of the group names and Id's.  group name is the key,  id is the value
        public Dictionary<string, string> GroupNameDictionary { get; set; }

        public List<BLL.GroupRole> GroupRoles { get; set; }
        public List<GroupSection> GroupSections { get; set; }
        public string GroupId { get; set; }

        public string GroupName { get; set; }
        public MxUser Usr { get; set; }
        public List<string> SortedNames { get; set; }

        public bool Adding { get; set; }
        

        /// <summary>
        /// Constructor for the Groups List view
        /// </summary>
        /// <param name="user"></param>
        /// <param name="adding"></param>
        public GroupsModel(MxUser user, bool adding)
        {
            GroupNameDictionary = BLL.Groups.GetGroupsDict();
            SortedNames = GroupNameDictionary.Keys.ToList();
            SortedNames.Sort();
            Usr = user;            
        }

        /// <summary>
        /// Constructor for the Group Edit view
        /// </summary>
        /// <param name="user"></param>
        /// <param name="groupId"></param>
        /// <param name="groupName"></param>
        /// <param name="adding"></param>
        public GroupsModel(MxUser user, string groupId, string groupName, bool adding)
        {
            Usr = user;
            GroupSections = new List<GroupSection>();
            GroupRoles = BLL.Groups.RolesForGroupList(new Guid(groupId));
            GroupName = groupName;
            GroupId = groupId;
            Adding = adding;
            foreach (BLL.GroupRole role in GroupRoles)
            {
                string [] names = role.RoleName.Split('_');
                GroupSection gs = new GroupSection();
                if (GroupSectionCreated(names[0], out gs))
                {
                    gs.Add(role);
                }
                else
                {
                    gs.Name = names[0];
                    gs.Description = role.Description;
                    gs.Add(role);
                    gs.Id = Guid.NewGuid().ToString();
                    GroupSections.Add(gs);
                }
            }
        }

        /// <summary>
        /// Determines if a GroupSection has been created yet
        /// </summary>
        /// <param name="name">The GroupSection that we are looking for</param>
        /// <param name="section">out parameter - returns the GroupSection if it exists</param>
        /// <returns>true if the GroupSection exists, false if not</returns>
        public bool GroupSectionCreated(string name, out GroupSection section)
        {
            section = new GroupSection();
            foreach (GroupSection gs in GroupSections)
            {
                if (gs.Name == name) 
                {
                    section = gs;
                    return true; 
                }
            }
            return false;
        }
    }

    /// <summary>
    /// Class that contains the roles for a group section
    /// </summary>
    public class GroupSection
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }

        public BLL.GroupRole AddRole { get; set; }
        public BLL.GroupRole EditRole { get; set; }
        public BLL.GroupRole DeleteRole { get; set; }
        public BLL.GroupRole ReportsRole { get; set; }
        public BLL.GroupRole ViewRole { get; set; }

        public void Add(BLL.GroupRole role)
        {
            string[] names = role.RoleName.Split('_');
            if (names.Length > 1)
            {
                switch (names[1].ToLower())
                {
                    case "add":
                        AddRole = role;
                        break;
                    case "edit":
                        EditRole = role;
                        break;
                    case "delete":
                        DeleteRole = role;
                        break;
                    case "view":
                        ViewRole = role;
                        break;
                    case "reports":
                        ReportsRole = role;
                        break;
                }
            }
        }        
    }    
}
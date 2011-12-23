using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


namespace site.Classes
{
    public class User
    {
        #region properties

        public string Name { get; private set; }
        public bool IsAuthenticated { get; private set; }
        public List<ModuleRights> ModuleRights { get; private set; }
        
        #endregion

        #region constructors

        public User()
        {
            IsAuthenticated = false;
        }

        #endregion

        #region methods

        //public static User Authenticate(string username, string password)
        //{
            //User user = new User();
            //DataTable userDt = UserTools.Verify(username, password);
            //if (userDt.Rows.Count > 0)
            //{
            //    DataRow uRow = userDt.Rows[0];
            //    user.Name = uRow["NAME"].ToString();
            //    user.IsAuthenticated = true;
                
            //}
            
        //    return user;
        //}
        #endregion
    }

    public class ModuleRights
    {
        public string ModuleName { get; set; }
        public bool SView { get; set; }
        public bool SEdit { get; set; }
        public bool SCopy { get; set; }
        public bool SAdd { get; set; }
        public bool SDelete { get; set; }
        public bool RView { get; set; }
        public bool RPrint{ get; set; }
        public bool RFax { get; set; }
        public bool RCreate { get; set; }
        public bool FinancialData { get; set; }
        public bool SAll { get; set; }
        public string Depts { get; set; }
        public bool REmail { get; set; }
        public bool RFile { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class Customer
    {
        public static DataTable GetCustomersForUser(Guid userId)
        {
            SqlParameter[] sqlParams = 
            { 
                new SqlParameter("@UserId", userId)
            };

            return DBHelper.GetTable("aspmnxSP_GetCustomers4User", sqlParams);
        }
    }
}

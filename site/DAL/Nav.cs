using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class Nav
    {
        public static DataTable GetSearchCategories()
        {
            SqlParameter[] sqlParams = null;

            return DBHelper.GetTable("MnxSearchGetCategoryView", sqlParams);
        }

        public static DataTable GetSearchSubCategories(string categoryId)
        {
            SqlParameter[] sqlParams = 
            { 
                new SqlParameter("@categoryId", categoryId)
            };

            return DBHelper.GetDataTable("MnxSearchGetTypeView", sqlParams);
        }

        public static DataSet GetSearchResults(string searchText, string searchType, string userId)
        {
            SqlParameter[] sqlParams = 
            { 
                new SqlParameter("@searchType", searchType),
                new SqlParameter("@searchTerm", searchText),
                new SqlParameter("@userId", userId)
            };

            return DBHelper.GetDataSet("MnxSearchGetResultsSP", sqlParams);
        }
    }
}

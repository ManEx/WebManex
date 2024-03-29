﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BLL
{
    public class Nav
    {
        public static DataTable GetSearchCategories()
        {
            return DAL.Nav.GetSearchCategories();
        }

        public static DataTable GetSearchSubCategories(string categoryId)
        {
            return DAL.Nav.GetSearchSubCategories(categoryId);
        }

        public static DataSet GetSearchResults(string searchText, string searchType, string userId)
        {
            return DAL.Nav.GetSearchResults(searchText, searchType, userId);
        }
    }
}

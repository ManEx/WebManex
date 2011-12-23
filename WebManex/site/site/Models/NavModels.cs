using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using site.Classes;
using System.Web.Mvc;

namespace site.Models.Nav
{
    public class NavSearchModel
    {
        public MenuItemListModel GenCategoryList { get; set; }

        public MxUser Usr { get; set; }

        public NavSearchModel(MxUser mxUser)
        {
            GenCategoryList = new MenuItemListModel(true, GetSearchMenu(string.Empty));
            this.Usr = mxUser;
        }


        #region functions
        public static List<MenuItem> GetSearchMenu(string id)
        {
            if (HttpContext.Current.Session["searchMenu" + id] != null)
            {
                return (List<MenuItem>)HttpContext.Current.Session["searchMenu" + id];
            }
            else
            {
                List<MenuItem> searchMenuItems = new List<MenuItem>();
                DataTable subCatDt = new DataTable();
                if (string.IsNullOrEmpty(id))
                {
                    subCatDt = BLL.Nav.GetSearchCategories();
                }
                else { subCatDt = BLL.Nav.GetSearchSubCategories(id); }
                DataView menuDv = subCatDt.DefaultView;
                menuDv.Sort = "listOrder";
                foreach (DataRowView dvRow in menuDv)
                {
                    if (string.IsNullOrEmpty(id))
                    {
                        searchMenuItems.Add(new MenuItem("Resources.Shared.Search", dvRow["sCatResourceId"].ToString(), dvRow["sCatResourceValue"].ToString(), dvRow["sCategoryId"].ToString()));
                    }
                    else
                    {
                        searchMenuItems.Add(new MenuItem("Resources.Shared.Search", dvRow["sTypeResourceId"].ToString(), dvRow["sTypeResourceValue"].ToString(), dvRow["sTypeId"].ToString()));
                    }
                }
                HttpContext.Current.Session["searchMenu" + id] = searchMenuItems;
                return searchMenuItems;
            }
        }
        #endregion
    }

    public class SearchResultModel
    {
        public List<SearchResultCategory> Categories { get; set; }
        public string FirstCategory { get; set; }
        public SearchResultModel(DataSet resultDs)
        {
            Categories = new List<SearchResultCategory>();
            
            foreach (DataTable dt in resultDs.Tables)
            {
                List<string> columns = new List<string>();
                //data columns start at index 4
                for(int i = 4; i < dt.Columns.Count; i++)
                {
                    columns.Add(Resources.Localizer.GetString("Resources.Shared.Search", dt.Columns[i].ColumnName, dt.Columns[i].ColumnName));
                }

                DataView dv = dt.DefaultView;
                dv.Sort = "group";
                
                foreach (DataRowView drv in dv)
                {
                    SearchResultCategory srCat = FindCategory(drv["table"].ToString());
                    if (srCat == null)
                    {
                        srCat = new SearchResultCategory(drv);
                        srCat.Columns = columns;
                        Categories.Add(srCat);
                    }
                    if (string.IsNullOrEmpty(FirstCategory))
                    {
                        FirstCategory = srCat.Id;
                    }
                    SearchResultItem srItem = new SearchResultItem(drv, dt.Columns.Count);
                    srCat.Items.Add(srItem); 
                }
            }
        }

        private SearchResultCategory FindCategory(string category)
        {
            foreach (SearchResultCategory srCat in Categories)
            {
                if (srCat.Name == category) { return srCat; }
            }
            return null;
        }
    }


    public class SearchResultCategory
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public List<string> Columns { get; set; }
        public List<SearchResultItem> Items { get; set; }
        

        public SearchResultCategory(DataRowView drv)
        {
            Items = new List<SearchResultItem>();
            Name = drv["table"].ToString();
            Id = Guid.NewGuid().ToString();
        }
        public SearchResultCategory() { }
    }

    public class SearchResultItem
    {
        public string Id { get; set; }
        public List<string> Results { get; set; }
        public string Link { get; set; }
        public string Group { get; set; }

        public SearchResultItem(DataRowView drv, int colCount)
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            string baseUrl = string.Format("{0}://{1}{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, urlHelper.Content("~"));
            Results = new List<string>();
            for (int i = 4; i < colCount; i++)
            {
                Results.Add(drv.Row.ItemArray[i].ToString());
            }

            string path = drv["link"].ToString().StartsWith("/") ? drv["link"].ToString().Substring(1) : drv["link"].ToString();
            Link = baseUrl + path;
            Group = drv["group"].ToString();
        }
        public SearchResultItem() { }

    }


    
}
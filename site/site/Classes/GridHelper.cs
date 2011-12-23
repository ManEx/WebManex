using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace site.Classes
{
    public class GridHelper
    {
        public static string CreateJson(DataTable dt, string primaryColName, List<string> colNames, int pageSize, int currentPage)
        {
            StringBuilder jsonSb = new StringBuilder();
            int numRecords = dt.Rows.Count;
            int numPages = 0;
            int i = 0;
            var result = numRecords % pageSize;
	        if (result == 0) {  numPages = numRecords / pageSize; }
	        else { numPages = numPages / pageSize + 1; }
            jsonSb.Append("{ ");
            jsonSb.AppendFormat("\"total\": \"{0}\", \"page\": \"{1}\", \"records\": \"{2}\",  \"rows\" : [", numPages, currentPage, numRecords);
                
            foreach (DataRow row in dt.Rows)
            {
                jsonSb.Append("{ ");
                if (primaryColName != "")
                {
                    jsonSb.AppendFormat("\"id\":\"{0}\", \"cell\": [", row[primaryColName].ToString());
                }
                else
                {
                    jsonSb.AppendFormat("\"id\":\"{0}\", \"cell\": [", i++.ToString());
                }
                foreach (string col in colNames)
                {
                    try
                    {
                        if (col.ToLower().Contains("date"))
                        {
                            try
                            {
                                jsonSb.AppendFormat("\"{0}\",", (DateTime.Parse(row[col].ToString()).ToShortDateString()));
                            }
                            catch
                            {
                                jsonSb.AppendFormat("\"{0}\",", row[col].ToString());
                            }
                        }
                        else
                        {
                            jsonSb.AppendFormat("\"{0}\",", row[col].ToString().Replace("\"", "&#34;").Replace("'", "&#39;"));
                        }
                    }
                    catch
                    {
                        jsonSb.Append("\" \",");
                    }
                }                
                jsonSb = jsonSb.Remove(jsonSb.ToString().Length - 1, 1);
                
                jsonSb.Append("]},");
            }
            if (jsonSb.ToString().EndsWith(","))
            {
                jsonSb = jsonSb.Remove(jsonSb.ToString().Length - 1, 1);
            }
            jsonSb.Append("]}"); 

            return jsonSb.ToString();
        }

        public static List<string> ToList(string[] colNames)
        {
            List<string> colList = new List<string>();
            for (int i = 0; i < colNames.Length; i++)
            {
                colList.Add(colNames[i]);
            }
            return colList;
        }
    }
}

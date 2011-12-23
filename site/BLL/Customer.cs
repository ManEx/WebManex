using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using System.Data;

namespace BLL
{
    public class Customer
    {
        public static Dictionary<string, string> GetCustomersForUserDictionary(Guid userId)
        {
            DataTable dt = DAL.Customer.GetCustomersForUser(userId);
            Dictionary<string, string> custDict = new Dictionary<string, string>();
            foreach (DataRow row in dt.Rows)
            {
                custDict.Add(row["Custno"].ToString(), row["custname"].ToString());
            }
            return custDict;
        }
        public static DataTable GetCustomersForUser(Guid userId)
        {
            return DAL.Customer.GetCustomersForUser(userId);
        }

        public bool AddCustomer(string custId, string userId)
        {
            try
            {
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveCustomer(string custId, string userId)
        {
            try
            {
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class Order
    {
        
        
        public static DataTable GetOrdersByStatus(string custId, string status, int daysClosed)
        {
            SqlParameter[] sqlParams = 
            { 
                new SqlParameter("@pcCustNo", custId),
                new SqlParameter("@pcStatus", status),
                new SqlParameter("@pnClosedDays", daysClosed)
            };

            return DBHelper.GetDataTable("spManexWebSOStatus", sqlParams);
        }

        public static DataSet GetDueDatesShipments(string orderId)
        {
            SqlParameter[] sqlParams = 
            { 
                new SqlParameter("@pcUniqueLn", orderId)
            };

            return DBHelper.GetDataSet("spManexWebSODueDatesShipments", sqlParams);
        }
    }
}

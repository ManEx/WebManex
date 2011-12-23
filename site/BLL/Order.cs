using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using System.Data;

namespace BLL
{
    public class Order
    {
        public static DataTable GetOrdersByStatus(string custId, string status, int daysClosed)
        {
            return DAL.Order.GetOrdersByStatus(custId, status, daysClosed);
        }

        public static DataSet GetDueDatesShipments(string orderId)
        {
            return DAL.Order.GetDueDatesShipments(orderId);
        }
    }
}

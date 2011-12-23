using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using site.Classes;
using System.Web.Security;
using System.Configuration;

namespace site.Models
{
    
    public class OrderStatus
    {
        public Dictionary<string, string> AssignedCustomers { get; set; }
        public MxUser Usr { get; set; }
        
        public OrderStatus(MxUser um)
        {
            Usr = um;
            AssignedCustomers = BLL.Customer.GetCustomersForUserDictionary((Guid)Membership.GetUser().ProviderUserKey);
        }
        public string GetOrders(string status, string companyId)
        {
            int days = 0;
            if (status == "Closed30") {days = 30;}
            else if (status == "Closed90") {days = 90;}
            
            DataTable oTable = BLL.Order.GetOrdersByStatus(companyId, status, days);

            string[] oCols = { "uniqueln", "part_no", "status", "Revision", "Descript", "Pono", "is_rma", "Sono", "line_no", "orderdate", "ord_qty", "shippedqty", "Balance", "Released" };

            return GridHelper.CreateJson(oTable, "uniqueln", GridHelper.ToList(oCols), 50, 1);
        }

        public string GetDueDatesShipments(string orderId)
        {
            DataSet ds = BLL.Order.GetDueDatesShipments(orderId);
            string[] ddCols = { "Due_dts", "Qty" };
            string[] shpCols = { "PackListNo", "ShipDate", "ShippedQty", "ShipVia", "WayBill", "InvoiceNo", "InvDate" };

            string dueDatesJson = GridHelper.CreateJson(ds.Tables[0], "", GridHelper.ToList(ddCols), 10, 1);
            string shipmentsJson = GridHelper.CreateJson(ds.Tables[1], "", GridHelper.ToList(shpCols), 10, 1);
            return string.Format("{0} \"duedates\": {1}, \"shipments\": {2}{3}", "{", dueDatesJson, shipmentsJson, "}");
        }
    }
}
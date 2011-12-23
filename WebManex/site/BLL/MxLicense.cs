using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BLL
{
    public class MxLicense
    {
        public static void ClearInactiveSeats()
        {
            DAL.MxLicense.ClearInactiveSeats();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="licenseType"></param>
        /// <returns>number of how many seats are taken</returns>
        public static int GetActiveSeatCount(string licenseType)
        {
            DataTable cCountDt = DAL.MxLicense.GetActiveSeatCount(licenseType);

            if (cCountDt.Rows.Count > 0)
            {
                return (int)cCountDt.Rows[0]["userCount"];
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="licenseType"></param>
        /// <returns>number of how many total seats are available for the license</returns>
        public static int GetLicenseSeatCount()
        {
            DataTable lCountDt = DAL.MxLicense.GetLicenseSeatCount();

            if (lCountDt.Rows.Count > 0)
            {
                BLL.EnDecrypt licDec = new EnDecrypt(Encoding.ASCII.GetBytes("1f352c073b6108d72d9810a30914dff4"),
                    Encoding.ASCII.GetBytes("0001020304050607"));

                byte [] encTextByteArray = (byte[])lCountDt.Rows[0]["Field145"];
                if (encTextByteArray != null)
                {
                    return int.Parse(licDec.DecryptFromByteArray(encTextByteArray));
                }
            }
            return -1;
        } 

        /// <summary>
        /// SeatCheck
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sessionId"></param>
        /// <returns>true if the user has a seat, false if there is no seat</returns>
        public static bool SeatCheck(string userId, string sessionId)
        {
            DataTable dt = DAL.MxLicense.SeatCheckAndUpdateActivity(userId, sessionId);
            if (dt.Rows.Count > 0)
            {
                if ((int)dt.Rows[0]["IsIn"] > 0) { return true; }
            }

            return false;
        }

        public static void SeatUser(string userId, string sessionId, string workStationId, string lastModule, string ipAddress)
        {
            DataTable rolesDt = DAL.MxLicense.SeatUser(userId, sessionId, workStationId, lastModule, ipAddress);
        }



        public static void UnseatUser(string userId, string sessionId)
        {
            DAL.MxLicense.UnseatUser(userId, sessionId);
        }
    }
}

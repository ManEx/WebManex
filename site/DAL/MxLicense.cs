using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class MxLicense
    {
        public static void ClearInactiveSeats()
        {
            string query = @"
                    DELETE FROM aspmnx_activeUsers
                       WHERE sessionId IN (
                           SELECT a.sessionId FROM aspmnx_activeUsers AS a INNER JOIN aspnet_Profile AS p ON a.fkuserId = p.UserId
                           WHERE (COALESCE(p.minuteLimit,5) <> 0) AND (DATEADD(minute, COALESCE(p.minuteLimit,5), a.lastActivityDate) < GETDATE())
                       ) ";

            DBHelper.CustomResultsQuery(query);
        }

        public static DataTable GetActiveSeatCount(string licenseType)
        {
            string query = string.Format(@"
                            SELECT COUNT(au.sessionId) AS userCount
                            FROM aspnet_Profile AS p INNER JOIN aspmnx_ActiveUsers AS au ON p.UserId = au.fkuserId
                            WHERE (p.LicenseType = '{0}')", licenseType);

            return DBHelper.CustomResultsQuery(query);
        }

        public static DataTable GetLicenseSeatCount()
        {
            SqlParameter[] sqlParams = null;

            return DBHelper.GetTable("MicssysSumInfoView", sqlParams);
        }

        public static DataTable SeatCheckAndUpdateActivity(string userId, string sessionId)
        {
            string query = string.Format(@"DECLARE @isIn int = 0
                SELECT      @isIn = COUNT(sessionId)
                FROM        aspmnx_activeUsers
                WHERE       (fkuserId = '{0}') AND (sessionID = '{1}')
                IF @isIn =1
                BEGIN
                UPDATE      aspmnx_activeUsers
                SET         lastActivityDate = GETDATE()
                WHERE       (fkuserId = '{0}') AND (sessionID = '{1}') 
                END
                SELECT @isIn AS IsIn", userId, sessionId);

            return DBHelper.CustomResultsQuery(query);
        }


        public static DataTable SeatUser(string userId, string sessionId, string workStationId, string lastModule,
            string ipAddress)
        {
            string query = string.Format(@"
                    INSERT INTO aspmnx_activeUsers (sessionId,fkUserId,lastActivityDate,workstationId,lastModule,ipaddress)
                    VALUES ('{1}','{0}',GETDATE(),'{2}','{3}','{4}')", userId, sessionId, workStationId, lastModule, ipAddress);

            return DBHelper.CustomResultsQuery(query);
        }

        public static void UnseatUser(string userId, string sessionId)
        {
            string query = string.Format(@"
                    IF '{1}' = ''
                    BEGIN
                        DELETE FROM aspmnx_activeUsers
                        WHERE (fkuserId = '{0}')
                    END
                    ELSE
                    BEGIN
                        DELETE FROM aspmnx_activeUsers
                        WHERE (fkuserId = '{0}') AND (sessionId = '{1}')
                    END", userId, sessionId);

            DBHelper.CustomResultsQuery(query);
        }
    }
}

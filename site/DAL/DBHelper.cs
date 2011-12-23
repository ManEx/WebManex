using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace DAL
{
    public class DBHelper
    {
        static SqlConnection _conn;
        static string _connStr = ConfigurationManager.ConnectionStrings["eManEx"].ToString();
        static SqlCommand _cmd;
        static SqlDataAdapter _dataAdapter = new SqlDataAdapter();

        public static DataTable GetTable(string sProcName, SqlParameter[] sqlParams)
        {
            using (_conn = new SqlConnection(_connStr))
            {
                _cmd = new SqlCommand(sProcName, _conn);

                _conn.Open();
                _cmd.CommandType = CommandType.StoredProcedure;

                if (sqlParams != null)
                {
                    _cmd.Parameters.AddRange(sqlParams);
                }

                _dataAdapter = new SqlDataAdapter(_cmd);

                DataTable results = new DataTable();

                _dataAdapter.Fill(results);
                _conn.Close();

                return results;
            }
        }

        public static DataTable CustomResultsQuery(string query)
        {
            using (_conn = new SqlConnection(_connStr))
            {
                _cmd = new SqlCommand(query, _conn);

                _conn.Open();
                _dataAdapter = new SqlDataAdapter(_cmd);

                DataTable results = new DataTable();

                _dataAdapter.Fill(results);
                _conn.Close();

                return results;
            }
        }

        public static SqlDataReader ExecuteReader(string procName, params SqlParameter[] _params)
        {
            using (_conn = new SqlConnection(_connStr))
            {
                SqlCommand cmd = _conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procName;
                foreach (SqlParameter sp in _params)
                {
                    cmd.Parameters.Add(sp);
                }

                _conn.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public static SqlDataReader ExecuteReader(string procName)
        {
            using (_conn = new SqlConnection(_connStr))
            {
                SqlCommand cmd = _conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procName;


                _conn.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }


        public static SqlDataAdapter ExecuteAdapter(string procName, params SqlParameter[] procParams)
        {
            using (_conn = new SqlConnection(_connStr))
            {
                SqlCommand cmd = _conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = procName;
                foreach (SqlParameter p in procParams)
                {
                    cmd.Parameters.Add(p);
                }
                _conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                return da;
            }
        }

        public static SqlDataAdapter ExecuteAdapter(string procName)
        {
            using (_conn = new SqlConnection(_connStr))
            {
                SqlCommand cmd = _conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = procName;

                _conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                return da;
            }
        }

        public static DataSet GetDataSet(string procName, params SqlParameter[] procParams)
        {
            using (_conn = new SqlConnection(_connStr))
            {
                SqlCommand cmd = _conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = procName;
                foreach (SqlParameter p in procParams)
                {
                    cmd.Parameters.Add(p);
                }
                _conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataSet ds = new DataSet();
                da.Fill(ds);

                return ds;

            }
        }

        public static DataTable GetDataTable(string procName, params SqlParameter[] procParams)
        {
            using (_conn = new SqlConnection(_connStr))
            {
                SqlCommand cmd = _conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = procName;
                foreach (SqlParameter p in procParams)
                {
                    cmd.Parameters.Add(p);
                }
                _conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable table = new DataTable();
                da.Fill(table);

                return table;

            }
        }


        public static object ExecuteScalar(string procName, params SqlParameter[] procParams)
        {
            using (_conn = new SqlConnection(_connStr))
            {
                SqlCommand cmd = _conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = procName;
                foreach (SqlParameter p in procParams)
                {
                    cmd.Parameters.Add(p);
                }
                _conn.Open();
                return cmd.ExecuteScalar();
            }
        }
        public static object ExecuteScalar_NoProc(string commandText)
        {
            using (_conn = new SqlConnection(_connStr))
            {
                SqlCommand cmd = _conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = commandText;

                _conn.Open();
                return cmd.ExecuteScalar();
            }
        }

        public static DataTable GetTableList()
        {
            using (_conn = new SqlConnection(_connStr))
            {
                _conn.Open();
                DataTable schemaTable = _conn.GetSchema("Tables");

                return schemaTable;
            }

        }
        public static int ExecuteNonQuery(string procName, params SqlParameter[] procParams)
        {

            using (_conn = new SqlConnection(_connStr))
            {
                SqlCommand cmd = _conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandText = procName;
                foreach (SqlParameter p in procParams)
                {
                    if (p.Value == null)
                    {
                        p.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(p);
                }
                SqlParameter param = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                param.Direction = ParameterDirection.ReturnValue;
                _conn.Open();

                cmd.ExecuteNonQuery();
                SqlParameter result = cmd.Parameters["@RETURN_VALUE"];
                if (result == null)
                    return 0;
                else
                    return (int)result.Value;
            }
        }
        public static int ExecuteNonQuery(string procName, DataTable dataTbl)
        {
            using (_conn = new SqlConnection(_connStr))
            {
                SqlCommand cmd = _conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procName;

                foreach (DataColumn column in dataTbl.Columns)
                {
                    cmd.Parameters.Add(new SqlParameter(column.ColumnName, dataTbl.Rows[0][column.ColumnName]));
                }
                SqlParameter param = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                param.Direction = ParameterDirection.ReturnValue;
                _conn.Open();

                cmd.ExecuteNonQuery();
                SqlParameter result = cmd.Parameters["@RETURN_VALUE"];
                return (result == null) ? 0 : (int)result.Value;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace AppCore
{
    public abstract class DBContext : IDBContext
    {
        public string ConnectionString { get; set; }
        public string DBType { get; set; }

        #region Methods and Subroutines
        public int ExecuteNonQuery(string cmdText, Dictionary<string, object> cmdParms)
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionString].ToString()))
            {
                SqlCommand cmd = conn.CreateCommand();
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }
        public int ExecuteNonQuery(CommandType cmdType, string cmdText, Dictionary<string, object> cmdParms)
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionString].ToString()))
            {
                SqlCommand cmd = conn.CreateCommand();
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }
        public IDataReader ExecuteReader(CommandType cmdType, string cmdText, Dictionary<string, object> cmdParms)
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionString].ToString()))
            {
                SqlCommand cmd = conn.CreateCommand();
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return rdr;
            }
        }
        public object ExecuteScalar(CommandType cmdType, string cmdText, Dictionary<string, object> cmdParms)
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionString].ToString()))
            {
                SqlCommand cmd = conn.CreateCommand();
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }
        private void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, Dictionary<string, object> cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                foreach (var param in cmdParms)
                {
                    var parameter = cmd.CreateParameter();
                    parameter.ParameterName = param.Key;
                    parameter.Value = param.Value;
                    cmd.Parameters.Add(parameter);
                }

            }
        }
        #endregion
    }
}
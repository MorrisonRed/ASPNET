using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;


namespace AppCore
{
    public interface IDBContext
    {
        string DBType { get; set; }
        string ConnectionString { get; set; }

        int ExecuteNonQuery(string cmdText, Dictionary<string, object> cmdParms);
        int ExecuteNonQuery(CommandType cmdType, string cmdText, Dictionary<string, object> cmdParms);
        IDataReader ExecuteReader(CommandType cmdType, string cmdText, Dictionary<string, object> cmdParms);
        object ExecuteScalar(CommandType cmdType, string cmdText, Dictionary<string, object> cmdParms);
        //void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, Dictionary<string, object> cmdParms);
    }
}

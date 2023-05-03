using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_DBManager
{
    public class LogDao
    {
        OracleConnectionPool connPool = OracleConnectionPool.Instance();

        public int InsertLog(LogVo vo)
        {
            int nRow = -1;
            string sql = @"INSERT INTO log(log_seq, member_id, log_ip, log_port, log_info) " +
                        @"VALUES(log_sequence.next_val, :mid, :ip, :port, :info)";

            OracleConnection conn = connPool.GetConnection();

            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.Parameters.Add("mid", vo.MEMBER_ID);
                cmd.Parameters.Add("ip", vo.LOG_IP);
                cmd.Parameters.Add("port", vo.LOG_PORT);
                cmd.Parameters.Add("info", vo.LOG_INFO);

                nRow = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                DBDebug.WriteLog($"DB Exception : {ex.Message}");
            }
            finally
            {
                connPool.ReturnOracleConnection(conn);
            }

            return nRow;
        }
    }
}

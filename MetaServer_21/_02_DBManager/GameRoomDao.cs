using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _02_DBManager
{
    public class GameRoomDao
    {
        OracleConnectionPool connPool = OracleConnectionPool.Instance();

        public GameRoomVo GetGameRoomData(string rid)
        {
            GameRoomVo vo = new GameRoomVo();

            string sql = @"SELECT * FROM gameroom WHERE gameroom_id = :rid";

            // 커넥션풀에서 오라클연결 객체를 대여
            OracleConnection conn = connPool.GetConnection();

            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.Parameters.Add("rid", rid);

                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    vo.GAMEROOM_ID = reader["gameroom_id"].ToString();
                    vo.GAMEROOM_NAME = reader["gameroom_name"].ToString();
                }
            }
            catch (Exception ex)
            {
                DBDebug.WriteLog($"DB Exception : {ex.Message}");
            }
            finally
            {
                // 커넥션풀에 오라클연결 객체를 반환
                connPool.ReturnOracleConnection(conn);
            }

            return vo;
        }

        public List<GameRoomVo> GetGameRoomList()
        {
            List<GameRoomVo> voList = new List<GameRoomVo>();

            string sql = "SELECT * FROM gameroom";

            OracleConnection conn = connPool.GetConnection();

            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;

                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    GameRoomVo vo = new GameRoomVo();
                    vo.GAMEROOM_ID = reader["gameroom_id"].ToString();
                    vo.GAMEROOM_NAME = reader["gameroom_name"].ToString();

                    voList.Add(vo);
                }
            }
            catch (Exception ex)
            {
                DBDebug.WriteLog($"DB Exception : {ex.Message}");
            }
            finally
            {
                connPool.ReturnOracleConnection(conn);
            }

            return voList;
        }
        public int InsertGameRoom(GameRoomVo vo)
        {
            int nRow = -1;
            string sql = @"INSERT INTO gameroom(gameroom_id, gameroom_name) " +
                        @"VALUES(:id, :name)";

            OracleConnection conn = connPool.GetConnection();

            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.Parameters.Add("id", vo.GAMEROOM_ID);
                cmd.Parameters.Add("name", vo.GAMEROOM_NAME);

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
        public int UpdateGameRoom(GameRoomVo vo, string rid)
        {
            int nRow = -1;
            string sql = @"UPDATE gameroom SET gameroom_id = :id, gameroom_name = :name " +
                        @"WHERE gameroom_id = :rid";

            OracleConnection conn = connPool.GetConnection();

            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.Parameters.Add("id", vo.GAMEROOM_ID);
                cmd.Parameters.Add("name", vo.GAMEROOM_NAME);
                cmd.Parameters.Add("rid", rid);

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
        public int DeleteGameRoom(string rid)
        {
            int nRow = -1;
            string sql = @"DELETE FROM gameroom WHERE gameroom_id = :rid";

            OracleConnection conn = connPool.GetConnection();

            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.Parameters.Add("rid", rid);

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

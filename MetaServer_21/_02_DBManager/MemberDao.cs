using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _02_DBManager
{
    public class MemberDao
    {
        OracleConnectionPool connPool = OracleConnectionPool.Instance();

        // id, password로 해당 멤버가 존재하는지 여부
        public MemberVo GetMemberData(string mid, string pass)
        {
            MemberVo memberVo = new MemberVo();

            string sql = @"SELECT * FROM member WHERE member_id=:mid AND member_pass=:pass";

            // 커넥션풀에서 오라클연결 객체를 대여
            OracleConnection conn = connPool.GetConnection();

            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection= conn;
                cmd.CommandText= sql;
                cmd.Parameters.Add("mid", mid);
                cmd.Parameters.Add("pass", pass);

                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    memberVo.MEMBER_ID = reader["member_id"].ToString();
                    memberVo.MEMBER_PASS = reader["member_pass"].ToString();
                    memberVo.MEMBER_NAME = reader["member_name"].ToString();
                    memberVo.MEMBER_AGE = reader["member_age"].ToString();
                    memberVo.MEMBER_JOB = reader["member_job"].ToString();
                    memberVo.MEMBER_PHONE = reader["member_phone"].ToString();
                    memberVo.GAMEROOM_ID = reader["gameroom_id"].ToString();
                }

            }catch(Exception ex)
            {
                DBDebug.WriteLog($"DB Exception : {ex.Message}");
            }
            finally
            {
                // 커넥션풀에 오라클연결 객체를 반환
                connPool.ReturnOracleConnection(conn);
            }

            return memberVo;
        }

        // 전체 회원 리스트 
        public List<MemberVo> GetMemberList()
        {
            List<MemberVo> voList = new List<MemberVo>();

            string sql = "SELECT * FROM member";

            OracleConnection conn = connPool.GetConnection();

            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;

                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    MemberVo memberVo = new MemberVo();
                    memberVo.MEMBER_ID = reader["member_id"].ToString();
                    memberVo.MEMBER_PASS = reader["member_pass"].ToString();
                    memberVo.MEMBER_NAME = reader["member_name"].ToString();
                    memberVo.MEMBER_AGE = reader["member_age"].ToString();
                    memberVo.MEMBER_JOB = reader["member_job"].ToString();
                    memberVo.MEMBER_PHONE = reader["member_phone"].ToString();
                    memberVo.GAMEROOM_ID = reader["gameroom_id"].ToString();

                    voList.Add(memberVo);
                }
            }catch(Exception ex)
            {
                DBDebug.WriteLog($"DB Exception : {ex.Message}");
            }
            finally
            {
                connPool.ReturnOracleConnection(conn);
            }

            return voList;
        }

        // 회원정보 저장
        public int InsertMember(MemberVo vo)
        {
            int nRow = -1;
            string sql = @"INSERT INTO member(member_id, member_pass, member_name, " +
                        @"member_age, member_job, member_phone, gameroom_id) " +
                        @"VALUES(:id, :pass, :name, :age, :job, :phone, :gid)";

            OracleConnection conn = connPool.GetConnection();

            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection= conn;
                cmd.CommandText= sql;
                cmd.Parameters.Add("id", vo.MEMBER_ID);
                cmd.Parameters.Add("pass", vo.MEMBER_PASS);
                cmd.Parameters.Add("name", vo.MEMBER_NAME);
                cmd.Parameters.Add("age", vo.MEMBER_AGE);
                cmd.Parameters.Add("job", vo.MEMBER_JOB);
                cmd.Parameters.Add("phone", vo.MEMBER_PHONE);
                cmd.Parameters.Add("gid", vo.GAMEROOM_ID);

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

        // 특정 멤버가 특정 게임룸에 진입
        public int UpdateMemberJoin(string mid, string gid)
        {
            int nRow = -1;
            string sql = @"UPDATE member SET " +
                        @"gameroom_id = :gid " +
                        @"WHERE member_id = :mid";

            OracleConnection conn = connPool.GetConnection();

            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText= sql;
                cmd.Parameters.Add("gid", gid);
                cmd.Parameters.Add("mid", mid);

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

        // 특정 멤버 데이터 수정
        public int UpdateMember(MemberVo vo, string mid)
        {
            int nRow = -1;
            string sql = @"UPDATE member SET member_id=:id, member_pass=:pass, member_name=:name, " +
                        @"member_age=:age, member_job=:job, member_phone=:phone, " +
                        @"gameroom_id=:gid " +
                        @"WHERE member_id=:mid";

            OracleConnection conn = connPool.GetConnection();

            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.Parameters.Add("id", vo.MEMBER_ID);
                cmd.Parameters.Add("pass", vo.MEMBER_PASS);
                cmd.Parameters.Add("name", vo.MEMBER_NAME);
                cmd.Parameters.Add("age", vo.MEMBER_AGE);
                cmd.Parameters.Add("job", vo.MEMBER_JOB);
                cmd.Parameters.Add("phone", vo.MEMBER_PHONE);
                cmd.Parameters.Add("gid", vo.GAMEROOM_ID);
                cmd.Parameters.Add("mid", mid);

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

        // 특정 멤버 데이터 삭제
        public int DeleteMember(string mid)
        {
            int nRow = -1;
            string sql = @"DELETE FROM member WHERE member_id = :mid";

            OracleConnection conn = connPool.GetConnection();

            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.Parameters.Add("mid", mid);

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

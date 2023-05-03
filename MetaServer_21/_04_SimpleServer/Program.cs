using FreeNet;
using NetworkDebug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04_SimpleServer
{
    internal class Program
    {
        // 클라이언트 접속시 생성할 CGameUser객체를 저장
        static List<CGameUser> userlist = new List<CGameUser>();

        static void Main(string[] args)
        {
            // FreeNet에서 내부적으로 사용할 패킷객체 2000개 할당
            CPacketBufferManager.initialize(2000);

            // 서버 가동
            CNetworkService service = new CNetworkService();
            // 클라이언트 접속시 호출될 메서드 등록
            service.session_created_callback += on_session_created;
            service.initialize();
            service.listen("0.0.0.0", 7979, 10000);

            SDebug.WriteLog("Server Started!");
            while (true)
            {
                string input = Console.ReadLine();
                if (input.Equals("1234567890"))
                    break;
                System.Threading.Thread.Sleep(1000);
            }

            SDebug.WriteLog("Server Ended!");
        }

        /// <summary>
        /// 클라이언트가 접속 완료했을 때 호출
        /// 1개 클라이언트당 1개 객체가 생성됨
        /// </summary>
        /// <param name="token">해당 클라이언트 통신담당 객체</param>
        static void on_session_created(CUserToken token)
        {
            CGameUser user = new CGameUser(token);
            user.on_remove_user_callback += on_remove_user;

            lock (userlist)
            {
                userlist.Add(user);
            }
        }

        /// <summary>
        /// 특정 클라이언트 접속 해제시 호출되야 하는 함수
        /// 클라이언트를 관리하는 리스트에서 제거
        /// </summary>
        /// <param name="user">클라이언트의 로직을 담당하는 CGameUser객체</param>
        public static void on_remove_user(CGameUser user)
        {
            lock (userlist)
            {
                SDebug.WriteLog($"{user} client Removed!");

                userlist.Remove(user);
            }
        }
    }
}

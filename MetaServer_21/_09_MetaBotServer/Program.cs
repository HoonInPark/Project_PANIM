using FreeNet;
using NetworkDebug;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _09_MetaBotServer
{
    internal class Program
    {
        static CProcessPacket procPacket = new CProcessPacket();
        static List<CGameUser> userlist = new List<CGameUser>();
        static CGameRoomManager gameRoomMgr = new CGameRoomManager();

        static void Main(string[] args)
        {
            procPacket.GAMEROOM_MGR = gameRoomMgr; // 만들어진 객체를 여기에 전달하여 GAMEROOM_MGR에 접근할 수 있게 만든다.
            
            CPacketBufferManager.initialize(2000);

            CNetworkService service = new CNetworkService();
            service.session_created_callback += on_session_created;
            service.initialize();
            service.listen("0.0.0.0", 7979, 10000);

            while (true)
            {
                string input = Console.ReadLine();
                if (input.Equals("exit"))
                    break;

                System.Threading.Thread.Sleep(1000);
            }

            SDebug.WriteLog("Server End");
        }

        static void on_session_created(CUserToken token)
        {
            CGameUser user = new CGameUser(procPacket, token);
            user.remove_user_callback += on_remove_user;

            SDebug.WriteLog($"{user} Client Connected");

            lock (userlist)
            {
                userlist.Add(user);
            }
        }

        static void on_remove_user(CGameUser user)
        {
            SDebug.WriteLog($"{user} Client Remove");

            lock (userlist)
            {
                userlist.Remove(user);
            }
        }
    }
}

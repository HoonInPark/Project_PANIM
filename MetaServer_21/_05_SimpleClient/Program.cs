using FreeNet;
using FreeNetProtocol;
using NetworkDebug;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace _05_SimpleClient
{
    internal class Program
    {
        // 클라이언트가 접속하는 서버들을 담당하는 객체 저장
        static List<IPeer> game_servers = new List<IPeer>();

        static int ShowMenu()
        {
            Console.WriteLine("1. 메시지 전송");
            Console.WriteLine("2. 좌표 전송");
            Console.WriteLine("3. ID/PASS 전송");
            int sel = int.Parse(Console.ReadLine());
            return sel;
        }

        static CPacket SendMsg()
        {
            Console.Write(">> ");
            string line = Console.ReadLine();
            CPacket msg = CPacket.create((short)PROTOCOL.CHAT_MSG_REQ);
            msg.push(line);
            return msg;
        }
        static CPacket SendPosition()
        {
            CPacket msg = CPacket.create((short)PROTOCOL.POSITION_REQ);
            float x = 0.1f;
            float y = 0.2f;
            float z = 0.3f;
            msg.push(x);
            msg.push(y);
            msg.push(z);
            return msg;
        }
        static CPacket SendIdPass()
        {
            CPacket msg = CPacket.create((short)PROTOCOL.ID_PASS_REQ);
            msg.push("asdf");
            msg.push("1234");
            return msg;
        }

        static void Main(string[] args)
        {
            CPacketBufferManager.initialize(2000);              // 통신데이터 저장공간 설정
            
            CNetworkService service = new CNetworkService();    // 통신관련 기능 설정
            CConnector connector = new CConnector(service);     // 서버에 접속역할 객체
            connector.connected_callback += on_connected_gameserver;
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7979);
            connector.connect(endpoint);        // 서버에 접속 요청

            while (true)
            {
                CPacket msg = null;
                int sel = ShowMenu();
                switch (sel)
                {
                    case 1:
                        msg = SendMsg();
                        break;
                    case 2:
                        msg = SendPosition();
                        break;
                    case 3:
                        msg = SendIdPass();
                        break;
                }

                // 1개 서버에만 접속하니까 등록된 1번째 CRemoteServerPeer객체를 통해
                // 서버에 데이터를 전송한다.
                game_servers[0].send(msg);
            }
        }

        /// <summary>
        /// 서버에 접속 성공시 호출되는 메서드
        /// </summary>
        /// <param name="server_token">서버와의 통신을 담당하는 객체</param>
        static void on_connected_gameserver(CUserToken server_token)
        {
            lock (game_servers)
            {
                CRemoteServerPeer server_peer = new CRemoteServerPeer(server_token);
                server_peer.on_remove_peer_callback += on_removed_peer;

                game_servers.Add(server_peer);
                SDebug.WriteLog("Connected!");
            }
        }

        /// <summary>
        /// 서버와의 접속이 해제되었을 때 호출되는 메서드
        /// </summary>
        /// <param name="server_peer">서버와의 비즈니스 로직을 담당하는 객체</param>
        static void on_removed_peer(CRemoteServerPeer server_peer)
        {
            lock (game_servers)
            {
                game_servers.Remove(server_peer);
                SDebug.WriteLog($"{server_peer} Removed");
            }
        }
    }
}

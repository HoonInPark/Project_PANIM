using FreeNet;
using FreeNetProtocol;
using NetworkDebug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04_SimpleServer
{
    // 접속 클라이언트 1개당 객체 1개(역할 : App, 비즈니스 로직처리)
    public class CGameUser : IPeer
    {
        // 접속 클라이언트 1개당 객체 1개(역할 : 통신 처리)
        CUserToken token;

        // 클라이언트 접속 해제시 호출될 이벤트 메서드
        public delegate void RemoveUserHandler(CGameUser user);
        public RemoveUserHandler on_remove_user_callback { get; set; }

        public CGameUser(CUserToken token)
        {
            //CUserToken <-> CGameUser에서 서로 접근 가능하도록 
            this.token = token;
            this.token.set_peer(this);
        }

        public void disconnect()
        {
            this.token.socket.Disconnect(false);
        }

        /// <summary>
        /// 클라이언트가 보낸 데이터를 수신하는 곳
        /// 수신 후 명령 처리
        /// </summary>
        /// <param name="buffer">수신 데이터</param>
        public void on_message(Const<byte[]> buffer)
        {
            // 클라이언트의 데이터를 CPacket객체로 전환
            CPacket msg = new CPacket(buffer.Value, this);

            process_user_operation(msg);
        }

        public void on_removed()
        {
            if(on_remove_user_callback!= null)
                on_remove_user_callback(this);
        }

        public void process_user_operation(CPacket msg)
        {
            CPacket ack = null;
            // CPacket객체에서 현재 프로토콜 종류를 추출
            PROTOCOL protocol = (PROTOCOL)msg.pop_protocol_id();
            switch (protocol)
            {
                case PROTOCOL.CHAT_MSG_REQ:
                    string line = msg.pop_string();
                    SDebug.WriteLog($"메시지 : {line}");
                    ack = CPacket.create((short)PROTOCOL.CHAT_MSG_ACK);
                    send(ack);
                    break;
                case PROTOCOL.POSITION_REQ:
                    float x = msg.pop_Float();
                    float y = msg.pop_Float();
                    float z = msg.pop_Float();
                    SDebug.WriteLog($"좌표 : x={x}, y={y}, z={z}");
                    ack = CPacket.create((short)PROTOCOL.POSITION_ACK);
                    send(ack);
                    break;
                case PROTOCOL.ID_PASS_REQ:
                    string id = msg.pop_string();
                    string pass = msg.pop_string();
                    SDebug.WriteLog($"ID/PASS : {id}/{pass}");
                    ack = CPacket.create((short)PROTOCOL.ID_PASS_ACK);
                    send(ack);
                    break;
            }
        }

        public void send(CPacket msg)
        {
            this.token.send(msg);
        }
    }
}

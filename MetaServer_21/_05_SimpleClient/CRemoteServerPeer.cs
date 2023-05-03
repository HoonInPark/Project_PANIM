using FreeNet;
using FreeNetProtocol;
using NetworkDebug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_SimpleClient
{
    internal class CRemoteServerPeer : IPeer
    {
        public CUserToken token { get; set; }

        // 서버 접속 해제시 호출될 이벤트 메서드
        public delegate void RemovePeerHandler(CRemoteServerPeer server_peer);
        public RemovePeerHandler on_remove_peer_callback { get; set; }

        public CRemoteServerPeer(CUserToken token)
        {
            //CUserToken <-> CRemoteServerPeer에서 서로 접근 가능하도록 
            this.token = token;
            this.token.set_peer(this);
        }

        public void disconnect()
        {
            this.token.socket.Disconnect(false);
        }

        public void on_message(Const<byte[]> buffer)
        {
            CPacket msg = new CPacket(buffer.Value, this);

            process_user_operation(msg);
        }

        public void on_removed()
        {
            if (on_remove_peer_callback != null)
                on_remove_peer_callback(this);
        }

        public void process_user_operation(CPacket msg)
        {
            PROTOCOL protocol_id = (PROTOCOL)msg.pop_protocol_id();
            switch(protocol_id)
            {
                case PROTOCOL.CHAT_MSG_ACK:
                    SDebug.WriteLog($"{PROTOCOL.CHAT_MSG_ACK.ToString()} OK");
                    break;
                case PROTOCOL.POSITION_ACK:
                    SDebug.WriteLog($"{PROTOCOL.POSITION_ACK.ToString()} OK");
                    break;
                case PROTOCOL.ID_PASS_ACK:
                    SDebug.WriteLog($"{PROTOCOL.ID_PASS_ACK.ToString()} OK");
                    break;
            }
        }

        public void send(CPacket msg)
        {
            this.token.send(msg);
        }
    }
}

using FreeNet;
using MetaBotServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _09_MetaBotServer
{
    // 패킷의 비즈니스 로직 분기를 위한 클라이언트
    public class CGameUser : IPeer
    {
        public CPlayer PLAYER { get; set; } // 게임룸 내에서의 클라이언트관련 정보를 담당.
        CProcessPacket procPacket; // 하나의 클래스로만 패킷을 처리하면 모듈화하기 힘들다.
        CUserToken token; // 패킷 처리를 해주는 곳.

        public delegate void Remove_user(CGameUser user);
        public Remove_user remove_user_callback { get; set; }

        public CGameUser(CProcessPacket procPacket, CUserToken token)
        {
            this.procPacket = procPacket;
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
            if (remove_user_callback != null)
                remove_user_callback(this);
        }

        public void process_user_operation(CPacket msg)
        {
            PROTOCOL protocol = (PROTOCOL)msg.pop_protocol_id();

            switch(protocol)
            {
                case PROTOCOL.REG_MEMBER_REQ: 
                    procPacket.Process_REG_MEMBER_REQ(msg);
                    break;
                case PROTOCOL.LOGIN_REQ:
                    procPacket.Process_LOGIN_REQ(msg);
                    break;
                case PROTOCOL.ROOM_LIST_REQ:
                    procPacket.Process_ROOM_LIST_REQ(msg);
                    break;
                case PROTOCOL.MAKE_ROOM_REQ:
                    procPacket.Process_MAKE_ROOM_REQ(msg);
                    break;
                case PROTOCOL.JOIN_ROOM_REQ:
                    procPacket.Process_JOINROOM_REQ(msg);
                    break;
            }
        }

        public void send(CPacket msg)
        {
            this.token.send(msg);
        }
    }
}

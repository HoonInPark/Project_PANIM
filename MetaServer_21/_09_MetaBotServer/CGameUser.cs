using FreeNet;
using MetaBotServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _09_MetaBotServer
{
    public class CGameUser : IPeer
    {
        CProcessPacket procPacket;
        CUserToken token;

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
            }
        }

        public void send(CPacket msg)
        {
            this.token.send(msg);
        }
    }
}

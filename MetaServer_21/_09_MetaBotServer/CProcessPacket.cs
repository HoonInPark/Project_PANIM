using _02_DBManager;
using FreeNet;
using MetaBotServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _09_MetaBotServer
{
    public class CProcessPacket
    {
        GameRoomDao gameDao = new GameRoomDao();
        MemberDao memDao = new MemberDao();

        public void Process_REG_MEMBER_REQ(CPacket msg)
        {
            // DB에 적용
            MemberVo vo = new MemberVo();
            vo.MEMBER_ID = msg.pop_string();
            vo.MEMBER_PASS = msg.pop_string();
            int row = memDao.InsertMember(vo);

            // Unity Client에 응답
            CPacket ack = CPacket.create((short)PROTOCOL.REG_MEMBER_ACK);
            if (row == 1)
                ack.push((byte)1);  // success
            else
                ack.push((byte)0);  // fail

            msg.owner.send(ack);
        }

        public void Process_LOGIN_REQ(CPacket msg)
        {
            string id = msg.pop_string();
            string pass = msg.pop_string();
            MemberVo vo = memDao.GetMemberData(id, pass);

            // Unity Client에 결과를 응답
            CPacket ack = CPacket.create((short)PROTOCOL.LOGIN_ACK);
            ack.push(id);
            if(id.Equals(vo.MEMBER_ID))
                ack.push((byte)1);  // success
            else
                ack.push((byte)0);  // fail        
            msg.owner.send(ack);
        }
    
        public void Process_ROOM_LIST_REQ(CPacket msg)
        {
            IList<GameRoomVo> voList = gameDao.GetGameRoomList();

            CPacket ack = CPacket.create((short)(PROTOCOL.ROOM_LIST_ACK));
            ack.push_int16((short)voList.Count);
            foreach(var vo in voList)
            {
                ack.push(vo.GAMEROOM_ID);
                ack.push(vo.GAMEROOM_NAME);
            }

            msg.owner.send(ack);
        }

        public void Process_MAKE_ROOM_REQ(CPacket msg)
        {
            string user_id = msg.pop_string();
            string gameroom_id = msg.pop_string();
            string gameroom_name = msg.pop_string();

            GameRoomVo vo = new GameRoomVo();
            vo.GAMEROOM_ID = gameroom_id;
            vo.GAMEROOM_NAME = gameroom_name;
            int row = gameDao.InsertGameRoom(vo);

            CPacket ack = CPacket.create((short)PROTOCOL.MAKE_ROOM_ACK);
            ack.push(user_id);
            ack.push(gameroom_id);
            if (row == 1)
                ack.push((byte)1);
            else
                ack.push((byte)0);

            msg.owner.send(ack);
        }
    }
}

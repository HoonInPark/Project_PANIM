using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBotServer
{
    public enum PROTOCOL : short
    {
        BEGIN = 0, 
        
        REG_MEMBER_REQ = 1,     // 회원가입 요청
        REG_MEMBER_ACK = 2,     // 회원가입 응답

        LOGIN_REQ      = 3,     // 로그인 요청
        LOGIN_ACK      = 4,     // 로그인 응답

        MAKE_ROOM_REQ   = 5,    // 방 생성 요청
        MAKE_ROOM_ACK   = 6,    // 방 생성 응답

        ROOM_LIST_REQ   = 7,    // 방 리스트 요청
        ROOM_LIST_ACK   = 8,    // 방 리스트 응답

        JOIN_ROOM_REQ   = 9,    // 방 참여 요청
        JOIN_ROOM_ACK   = 10,   // 방 참여 응답
        
        END
    }
}

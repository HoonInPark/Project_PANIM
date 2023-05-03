using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeNetProtocol
{
    // 프로토콜 : 서버와 클라이언트간의 특정 약속
    public enum PROTOCOL : short
    {
        BEGIN = 0,

        CHAT_MSG_REQ = 1,
        CHAT_MSG_ACK = 2,

        POSITION_REQ = 3,
        POSITION_ACK = 4,

        ID_PASS_REQ = 5,
        ID_PASS_ACK = 6,

        END
    }
}

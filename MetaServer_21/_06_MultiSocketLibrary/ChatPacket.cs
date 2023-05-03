using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06_MultiClientSocketLibrary
{
    public class ChatPacket : CmdPacket
    {
        public string ID { get; set; }
        public string CHATDATA { get; set; }
    }
}

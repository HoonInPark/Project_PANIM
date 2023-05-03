using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_DBManager
{
    public class GameRoomVo
    {
        public string GAMEROOM_ID { get; set; }
        public string GAMEROOM_NAME { get; set; }

        public override string ToString()
        {
            return $"gameroom_id={GAMEROOM_ID}, " +
                   $"gameroom_name={GAMEROOM_NAME}";
        }
    }
}

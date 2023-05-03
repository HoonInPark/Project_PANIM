using System.Collections.Generic;

namespace _09_MetaBotServer
{
    // 여기서는 특정 룸에 들어온 유저들을 관리하고 동기화 해주는 기능을 구현할 것이다. 
    // 게임 룸 자체를 구동시키는 것이다.
    public class CGameRoom
    {
        public string GAMEROOM_ID { get; set; } // 게임룸 ID
        public string GAMEROOM_NAME { get; set; } // 게임룸 NAME

        List<CPlayer> playerList = new List<CPlayer>(); // 진입한 Player(Client)들을 리스트로 관리한다.
        
        public CGameRoom(string gmaeRoom_id, string gameRoom_name)
        {
            this.GAMEROOM_ID = gmaeRoom_id;
            this.GAMEROOM_NAME = gameRoom_name;
        }

        public void AddPlayerList(CPlayer player)
        {
            
        }
        
        public void RemovePlayerList(CPlayer player)
        {
            this.playerList.Remove(player);
        }
    }
}
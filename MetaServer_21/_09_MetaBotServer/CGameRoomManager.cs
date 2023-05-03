using System.Collections.Generic;
using _02_DBManager;

namespace _09_MetaBotServer
{
    // 게임 룸들을 (위에서) 등록/관리하는 클래스이다.
    // 항상 DB에 동기화를 해주는 것은 비효율적이다. 
    // 따라서 여기서는 변화가 생길 때마다 DB에 올리는 로직을 구현할 것이다.
    public class CGameRoomManager
    {
        GameRoomDao roomDao = new GameRoomDao();

        private List<CGameRoom> roomList = new List<CGameRoom>(); // 게임룸을 객체로 저장하는 곳
        
        public CGameRoomManager()
        {
            // 시작하자 마자 데이터베이스에 있는 게임룸 정보를 가져오고
            // 이를 CGameRoom 객체를 생성하여 리스트에 저장한다.

            IList<GameRoomVo> voList = roomDao.GetGameRoomList();

            foreach (var vo in voList)
            {
                CGameRoom gameRoom = new CGameRoom(vo.GAMEROOM_ID, vo.GAMEROOM_NAME);
                roomList.Add(gameRoom);
            }
        }
        
        public void AddRoomList(CGameRoom gameRoom)
        {
            roomList.Add(gameRoom);
        }
        
        public void RemoveRoomList(CGameRoom gameRoom)
        {
            roomList.Remove(gameRoom);
        }
        
        // 게임룸에 진입하면 CPlayer 객체를 만들어서 해당 게임룸에 등록시키는 메서드 (이건 책에 안나온다!)
        public void AddRoomCreatePlayer(string gameroom_id, string gameroom_name, CGameUser gameUser, string user_id)
        {
            foreach (var room in roomList)
            {
                // 게임룸의 id, name이 일치하는 room 객체를 찾아서 해당 room에 CPlayer 객체를 등록한다.
                if (room.GAMEROOM_ID.Equals(gameroom_id) && room.GAMEROOM_NAME.Equals(gameroom_name))
                {
                    CPlayer player = new CPlayer(room, gameUser, user_id);
                    gameUser.PLAYER = player;
                    room.AddPlayerList(player);
                }
            }
        }
    }
}
namespace _09_MetaBotServer
{
    // 게임 룸에 진입한 게임 상에서의 클라이언트
    public class CPlayer
    {
        public CGameRoom GAME_ROOM { get; set; } // 소속 룸을 의미
        public CGameUser GAME_USER { get; set; } // 연결된 CGameUser 객체를 의미
        public string USER_ID { get; set; } // Player의 정보를 의미한다.

        public CPlayer(CGameRoom gameRoom, CGameUser gameUser, string user_id)
        {
            this.GAME_ROOM = gameRoom;
            this.GAME_USER = gameUser;
            this.USER_ID = user_id;
            this.GAME_USER.PLAYER = this;
        }
    }
}
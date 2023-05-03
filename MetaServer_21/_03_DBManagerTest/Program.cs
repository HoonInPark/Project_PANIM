using _02_DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_DBManagerTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MemberDao mDao = new MemberDao();
            List<MemberVo> voList = mDao.GetMemberList();
            foreach(var vo in voList)
            {
                DBDebug.WriteLog(vo.ToString());
            }

            GameRoomDao gDao = new GameRoomDao();
            List<GameRoomVo> gvoList = gDao.GetGameRoomList();
            foreach (var gvo in gvoList)
            {
                DBDebug.WriteLog(gvo.ToString());
            }
        }
    }
}

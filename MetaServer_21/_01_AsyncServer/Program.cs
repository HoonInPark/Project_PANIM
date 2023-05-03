using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_AsyncServer
{
    internal class Program
    {
        static bool isDebug = true;

        public static void WriteLog(string message)
        {
            if(isDebug)
            {
                Console.WriteLine(message);
            }
        }

        static void Main(string[] args)
        {
            const int PORT = 9000;

            AsyncServer server = new AsyncServer(PORT);

            while (true)
            {
                string input = Console.ReadLine();
                if (input.Equals("1234567890"))
                    break;
                System.Threading.Thread.Sleep(1000);
            }

            WriteLog("서버 종료");
        }
    }
}

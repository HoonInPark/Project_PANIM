using _06_MultiClientSocketLibrary;
using NetworkDebug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08_SyncClientExe
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SyncClientSocket syncClient = new SyncClientSocket();
            syncClient.OnReceiveDataCallback += ReceiveData;
            syncClient.InitSocket("127.0.0.1", 9000);

            // 서버로 데이터 전송
            while (true)
            {
                string input = Console.ReadLine();
                if (input.Equals("exit"))
                {
                    syncClient.Disconnect();
                    break;
                }                    
                else
                    syncClient.SendData(input);
            }
        }

        static void ReceiveData(string data)
        {
            SDebug.WriteLog($"DLL로부터 수신 : {data}");
        }
    }
}

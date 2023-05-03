using _06_MultiClientSocketLibrary;
using NetworkDebug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _07_AsyncClientExe
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ASyncClientSocket aClient = new ASyncClientSocket("127.0.0.1", 9000);
            aClient.OnReceiveDataCallback += ReceiveData;

            // 서버로 데이터 전송
            while(true)
            {
                string input = Console.ReadLine();
                if (input.Equals("exit"))                   
                    break;
                else
                    aClient.SendProcess(input);
            }
        }

        // 서버로부터 데이터 수신
        static void ReceiveData(string data)
        {
            SDebug.WriteLog($"DLL로부터 수신 : {data}");
        }
    }
}

using NetworkDebug;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace _06_MultiClientSocketLibrary
{
    public class SyncClientSocket
    {
        Socket clientSocket;
        IPEndPoint ipep;                // 서버의 주소
        bool isRunRecv = true;          // 수신 스레드의 동작 여부

        /* TCP 패킷의 끝을 \n문자로 처리한다.
         * 전송시 패킷의 끝에 \n을 추가하고, (sw.WriteLine())
         * 수신시 TCP스택에서는 \n의 값을 읽었을때 해당 앞부분까지 데이터를
         * App에 전달한다.(sr.ReadLine())
         */
        NetworkStream ns;
        StreamWriter sw;
        StreamReader sr;

        string ID { get; set; }

        public delegate void Receive_Data_Handler(string data);
        public Receive_Data_Handler OnReceiveDataCallback = null;

        public SyncClientSocket() 
        {
#if UNITY_STANDALONE_WIN
#else
            SetID();
#endif
        }

        public SyncClientSocket(string ip, int port)
        {
#if UNITY_STANDALONE_WIN
#else
            SetID();
#endif
            InitSocket(ip, port);
        }

        public void SetID()
        {
            Console.Write("ID >> ");
            string id = Console.ReadLine();
            this.ID = id;
        }
        public void SetID(string id)
        {
            this.ID = id;
        }

        public void InitSocket(string ip, int port)
        {
            this.clientSocket = new Socket(AddressFamily.InterNetwork,
                                            SocketType.Stream, 
                                            ProtocolType.Tcp);
            this.ipep = new IPEndPoint(IPAddress.Parse(ip), port);

            SDebug.WriteLog("서버 접속 시도...");
            this.clientSocket.Connect(ipep);
            SDebug.WriteLog("서버 접속 연결!!!");

            this.ns = new NetworkStream(this.clientSocket);
            this.sw = new StreamWriter(this.ns);
            this.sr = new StreamReader(this.ns);

            this.isRunRecv = true;
            Thread threadRecv = new Thread(new ThreadStart(ThreadRecv));
            threadRecv.Start();
        }

        // 데이터 수신을 담당하는 스레드
        void ThreadRecv()
        {
            while (this.isRunRecv)
            {
                string data = null;
                try
                {
                    data = sr.ReadLine();   // TCP스택버퍼에 '\n'가 있으면 읽어서 리턴

                    SDebug.WriteLog($"==> 수신 : {data}");
                    if(data == null)        // 서버가 Close()호출한 경우
                        this.isRunRecv = false;
                    else
                    {
                        // 모든 패킷 클래스의 부모 클래스이므로
                        // 일단 CmdPacket으로 변환을 하면 CMD에 따라
                        // 어떤 자식 클래스로 변환해야 하는 지 알 수 있다.
                        CmdPacket cmd = JsonSerializer.Deserialize<CmdPacket>(data);
                        switch(cmd.CMD)
                        {
                            case 'C':
                                ChatPacket cp = JsonSerializer.Deserialize<ChatPacket>(data);
                                string recvData = String.Format($"{cp.ID} >> {cp.CHATDATA}");
                                if(OnReceiveDataCallback!= null)
                                    OnReceiveDataCallback(recvData);
                                //SDebug.WriteLog(recvData);
                                break;
                        }
                    }
                }
                catch (JsonException e)
                {
                    SDebug.WriteLog($"Json Exception : {e.Message}");
                    SDebug.WriteLog($"Json Data : {data}");
                }
                catch (Exception e)
                {
                    SDebug.WriteLog($"Recv Exception : {e.Message}");

                    this.isRunRecv = false;
                }
            }
        }

        // 전송을 담당하는 메서드
        public void SendData(string chatData)
        {
            ChatPacket cp = new ChatPacket();
            cp.CMD = 'C';
            cp.ID = this.ID;
            cp.CHATDATA = chatData;

            // json문자열로 변환
            string data = JsonSerializer.Serialize(cp);
            this.sw.WriteLine(data);    // 데이터 + '\n' 전송
            this.sw.Flush();
            SDebug.WriteLog($"전송 : {cp.ID} / {cp.CHATDATA}");
        }

        // 접속종료를 담당하는 메서드
        public void Disconnect()
        {
            if(this.clientSocket != null && this.clientSocket.Connected)
            {
                this.clientSocket.Close();
            }
        }
    }
}

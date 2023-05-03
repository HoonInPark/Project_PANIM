using NetworkDebug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _06_MultiClientSocketLibrary
{
    public class ASyncClientSocket
    {
        IPEndPoint ipep;                // 접속할 서버의 주소
        const int BUFFER_SIZE = 4096;
        Socket clientSocket = null;
        byte[] receive_buffer = new byte[BUFFER_SIZE];  // 수신 데이터가 저장될 공간

        public delegate void Receive_Data_Handler(string data);
        public Receive_Data_Handler OnReceiveDataCallback = null;

        public ASyncClientSocket()
        {
        }
        public ASyncClientSocket(string ip, int port)
        {
            InitClientSocket(ip, port);
        }

        public void InitClientSocket(string ip, int port)
        {
            this.clientSocket = new Socket(AddressFamily.InterNetwork,
                                            SocketType.Stream,
                                            ProtocolType.Tcp);
            this.ipep = new IPEndPoint(IPAddress.Parse(ip), port);

            SDebug.WriteLog("서버 접속 시도...");
            this.clientSocket.Connect(this.ipep);
            SDebug.WriteLog("서버 접속 연결!!!");

            // 서버에서 보내온 데이터를 수신할 수 있도록 비동기 방식으로 .NET에 등록한다.
            ReceiveProcess();
        }

        public void ReceiveProcess()
        {
            // 비동기 방식으로 수신되면 닷넷한테 메서드 호출을 의뢰함.
            // 닷넷에 데이터가 들어오면 ReceiveData메서드를 호출해줘
            // 수신 데이터는 receive_buffer에 담아줘

            // .NET 내부에 데이터수신스레드 할당
            // 해당 스레드에서 데이터 수신시 ReceiveData를 호출
            clientSocket.BeginReceive(receive_buffer, 0, BUFFER_SIZE,
                SocketFlags.None, new AsyncCallback(ReceiveData), clientSocket);
        }

        void ReceiveData(IAsyncResult iar)
        {
            // this.clientSocket과 동일하다.
            // BeginReceive메서드 호출시 넘겨준 파라미터를 아래처럼 받을 수 있다.
            Socket client = (Socket)iar.AsyncState;
            try
            {
                // 비동기 수신 처리 완료를 닷넷에 알려줌
                int recv = client.EndReceive(iar);
                // 0 byte를 수신했다는 것은 클라이언트가 접속 종료했다는 의미
                if (recv == 0)
                {
                    client.Close();
                }
                // 정상 데이터 수신시 byte[]을 string으로 변환하고 로그 출력
                else
                {
                    string receiveData = Encoding.UTF8.GetString(receive_buffer, 0, recv);
                    if (OnReceiveDataCallback != null)
                        OnReceiveDataCallback(receiveData);
                    //SDebug.WriteLog($"{client} = {receiveData}");

                    // 또 다시 연결된 클라이언트로부터 비동기 수신 .NET에 의뢰
                    ReceiveProcess();
                }
            }
            catch (Exception ex)
            {
                client.Close();
                SDebug.WriteLog($"CloseSocket = {ex.Message}");
            }
        }

        public void SendProcess(byte[] buffer, int len)
        {
            clientSocket.BeginSend(buffer, 0, len, SocketFlags.None,
                new AsyncCallback(SendData), clientSocket);
        }

        public void SendProcess(string data)
        {
            // string -> byte[]로 변환
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            SendProcess(buffer, buffer.Length);
        }

        void SendData(IAsyncResult iar)
        {
            Socket client = (Socket)iar.AsyncState;
            int sent = client.EndSend(iar);
            SDebug.WriteLog($"{client} send Data {sent} bytes");
        }

        public void CloseSocket()
        {
            this.clientSocket.Close();
            SDebug.WriteLog("Close Socket!");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _01_AsyncServer
{
    public class AsyncClient
    {
        AsyncServer aServer = null;
        const int BUFFER_SIZE = 4096;
        Socket clientSocket = null;
        byte[] receive_buffer = new byte[BUFFER_SIZE];  // 수신 데이터가 저장될 공간

        public AsyncClient(Socket clientSocket, AsyncServer aServer) 
        { 
            this.clientSocket = clientSocket;
            this.aServer = aServer;
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
                if(recv == 0)
                {
                    client.Close();
                    aServer.RemoveClient(this);
                }
                // 정상 데이터 수신시 byte[]을 string으로 변환하고 로그 출력
                else
                {
                    string receiveData = Encoding.UTF8.GetString(receive_buffer, 0, recv);
                    Program.WriteLog($"{client} = {receiveData}");

                    aServer.BroadCast(receive_buffer, recv, this);

                    // 또 다시 연결된 클라이언트로부터 비동기 수신 .NET에 의뢰
                    ReceiveProcess();
                }
            }catch(Exception ex)
            {
                client.Close();
                aServer.RemoveClient(this);
                Program.WriteLog($"CloseSocket = {ex.Message}");
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
            Program.WriteLog($"{client} send Data {sent} bytes");
        }
    }
}

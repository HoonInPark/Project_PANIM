using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _01_AsyncServer
{
    public class AsyncServer
    {
        Socket serverSocket = null;
        List<AsyncClient> clientList = new List<AsyncClient>();

        public AsyncServer(int port)
        {
            init(port);
        }

        public void init(int port)
        {
            // IPv4(32bit주소체계), TCP 프로토콜 사용(신뢰성있는 전달)
            serverSocket = new Socket(AddressFamily.InterNetwork,
                                    SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
            serverSocket.Bind(ipep);    // 소켓에 주소 부여(휴대폰에 번호 개통)
            serverSocket.Listen(1000);  // 동시에 클라이언트가 접속시 잠깐 대기시키는 Queue공간
            /*
             * 동기 방식 접속 대기(현재 이 서버에서는 사용하지 않음)
            serverSocket.Accept();      // 접속할 때까지 이 메서드가 리턴하지 않고 무한 대기
            */
            /* 비동기 방식 접속 대기
             * 이곳에서 대기하지 않고 
             * 접속이 이루어지면 .NET이 AcceptCon메서드를 자동으로 호출해준다.
             * 
             * .NET에서는 Accept를 처리할 스레드를 할당
             * 닷넷 내부의 접속수신스레드에 의해 AccetCon메서드 호출
             */
            serverSocket.BeginAccept(new AsyncCallback(AcceptCon), serverSocket);
        }

        // 비동기 방식일 때 클라이언트 접속시 자동 호출되어 접속처리를 하는 메서드
        void AcceptCon(IAsyncResult iar)
        {
            // BeginAccept할 때 넘겨준 serverSocket을 이렇게 받을 수 있다.
            Socket paramSocket = (Socket)iar.AsyncState;

            // 비동기 Accept 처리 완료, 클라이언트와 직접 통신 가능한 소켓 반환
            Socket clientSocket = paramSocket.EndAccept(iar);
            Program.WriteLog($"Connected to: {clientSocket.RemoteEndPoint.ToString()}");

            // 클라이언트와 통신하는 역할을 하는 클래스 객체 생성
            AsyncClient client = new AsyncClient(clientSocket, this);

            // 접속된 소켓을 가진 객체를 리스트 저장한다
            // 나중에 특정 클라이언트가 보내온 메시지를 연결된 모든 클라이언트한테
            // broadcasting하기 위해서
            /* 닷넷내부의 스레드에 의해 접속연결시 AcceptCon은 자동호출된다.
             * 나중에 접속이 끊어질 때는 다른 스레드에서 Remove처리를 해야 하므로
             * 스레드 동기화가 필요하다.
             * 아래처럼 하면 1개 스레드가 사용중일 때는 다른 스레드는 접근하지 못한다.
             */
            lock(clientList)
            {
                clientList.Add(client);
            }

            //또 다른 클라이언트의 접속을 허용하기 위해 다시 Accept를 닷넷에 걸어둔다
            serverSocket.BeginAccept(new AsyncCallback(AcceptCon), serverSocket);

            // 클라이언트는 수신을 하기 위한 처리를 한다.
            client.ReceiveProcess();
        }

        public void RemoveClient(AsyncClient client)
        {
            lock(clientList)
            {
                clientList.Remove(client);
            }
        }

        public void BroadCast(byte[] buffer, int len, AsyncClient exceptClient)
        {
            lock (clientList)
            {
                // 모든 Client한테 전송
                if(exceptClient == null)
                {
                    foreach(var client in clientList)
                        client.SendProcess(buffer, len);
                }
                // exceptClient만 제외하고 나머지 모두 전송
                else
                {
                    foreach (var client in clientList)
                    {
                        if (exceptClient != client)
                            client.SendProcess(buffer, len);
                    }                      
                }                
            }
        }

    }
}

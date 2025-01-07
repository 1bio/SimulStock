namespace GameServer;

using System.Net;
using System.Net.Sockets;

class Server
{
    public static int MaxPlayers { get; private set; }
    public static int Port { get; private set; }
    public static Dictionary<int, Client> clients = new Dictionary<int, Client>(); // 클라이언트 수, 객체 데이터
    public delegate void PacketHandler(int _fromClient, Packet _packet);
    public static Dictionary<int, PacketHandler> packetHandlers;

    private static TcpListener tcpListener;
    private static UdpClient udpListener;


    // 서버 초기화 및 실행(플레이어 수, 포트 번호)
    public static void Start(int _MaxPlayers, int _port)
    {
        MaxPlayers = _MaxPlayers; 
        Port = _port;

        System.Console.WriteLine("Starting Server...");
        InitializeServerData();

        // 클라이언트 객체 생성 및 서버 연결 요청 준비
        tcpListener = new TcpListener(IPAddress.Any, Port); 
        tcpListener.Start();

        // 클라이언트 연결 대기
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallblack), null);  // 클라이언트 연결 요청을 비동기적으로 대기
                                                                                         // 연결을 시도할 시 콜백 메서드(대리자) 실행, 사용자 상태 정보가 필요하지 않음
        udpListener = new UdpClient(Port);
        udpListener.BeginReceive(UDPReceiveCallback, null);

        System.Console.WriteLine($"Server started on {Port}.");
    } 

 
    #region TCP Methods
    // 비동기 클라이언트 연결 요청 처리 콜백
    private static void TCPConnectCallblack(IAsyncResult _result)
    {
        // 들어오는 연결 시도를 비동기적으로 받아들이고 원격 호스트 통신을 처리할 새로운 TcpClient 생성
        TcpClient _client = tcpListener.EndAcceptTcpClient(_result); 

        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallblack), null);
        System.Console.WriteLine($"Incoming connection from {_client.Client.RemoteEndPoint}..."); // 연결된 클라이언트의 IP 주소 및 포트 가져오기

        // 클라이언트 연결 처리
        for (int i = 1; i <= MaxPlayers; i++)
        {
            if(clients[i].tcp.socket == null){
                clients[i].tcp.Connect(_client); // 클라이언트 연결
                return;
            }
        }

        System.Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect: server full!");
    }
    #endregion


    #region UDP Methods
    // 클라이언트로부터 들어오는 UDP 패킷을 처리하는 콜백
    private static void UDPReceiveCallback(IAsyncResult _result)
    {
        try
        {
            IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] _data = udpListener.EndReceive(_result, ref _clientEndPoint); // 비동기적으로 클라이언트로부터 데이터 수신
            udpListener.BeginReceive(UDPReceiveCallback, null);

            if (_data.Length < 4)
            {
                return;
            }

            using (Packet _packet = new Packet(_data)) // 수신된 바이트 데이터를 객체로 변환
            {
                int _clientId = _packet.ReadInt(); // 패킷 첫 번째 값 ID 읽기

                if (_clientId == 0)
                {
                    return;
                }

                // 클라이언트가 처음 연결되는 상태면
                if (clients[_clientId].udp.endPoint == null) 
                {
                    clients[_clientId].udp.Connect(_clientEndPoint); 
                    return;
                }

                // 수신된 데이터가 올바른 클라이언트의 endPoint인지(Equals() 메서드 권장)
                if (clients[_clientId].udp.endPoint.ToString() == _clientEndPoint.ToString())
                {
                    clients[_clientId].udp.HandleData(_packet);
                }
            }
        }
        catch (Exception _ex)
        {
            Console.WriteLine($"Error receiving UDP data: {_ex}");
        }
    }

    // UDP 데이터를 클라이언트로 전송
    public static void SendUDPData(IPEndPoint _clientEndPoint, Packet _packet)
    {
        try
        {
            if (_clientEndPoint != null)
            {
                udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null); // 클라이언트로 UDP 데이터를 비동기적으로 전송 
            }
        }
        catch (Exception _ex)
        {
            Console.WriteLine($"Error sending data to {_clientEndPoint} via UDP: {_ex}");
        }
    }

    // 클라이언트 객체 수 만큼 초기화
    private static void InitializeServerData()
    {
        for (int i = 1; i <= MaxPlayers; i++)
        {
            clients.Add(i, new Client(i));
        }

        packetHandlers = new Dictionary<int, PacketHandler>(){
            { (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
            { (int)ClientPackets.playerMovement, ServerHandle.PlayerMovement },
            { (int)ClientPackets.animationState, ServerHandle.PlayerAnimationState },
            { (int)ClientPackets.helloMessage, ServerHandle.HelloMessage }
        };

        System.Console.WriteLine("Initialzed packets");
    }
    #endregion
}

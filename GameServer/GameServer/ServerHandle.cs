using System;
using System.Numerics;
using GameServer;

// 클라이언트 -> 서버 보낸 패킷 데이터 처리
class ServerHandle
{
    // Welcome 출력
    public static void WelcomeReceived(int _fromClinet, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();

        Console.WriteLine($"{Server.clients[_fromClinet].tcp.socket.Client.RemoteEndPoint} 서버에 성공적으로 연결했습니다. 접속한 플레이어 인원: {_fromClinet}.");
        if (_fromClinet != _clientIdCheck)
        {
            Console.WriteLine($"Player \"{_username}\" (ID: {_fromClinet}) 잘못된 클라이언트 ID 입니다. ({_clientIdCheck})!");
        }

        Server.clients[_fromClinet].SendIntoGame(_username); 
    }

    // 플레이어 이동
    public static void PlayerMovement(int _fromClient, Packet _packet)
    {
        bool[] _inputs = new bool[_packet.ReadInt()];

        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }

        Quaternion _rotation = _packet.ReadQuaternion();

        Server.clients[_fromClient].player.SetInput(_inputs, _rotation);
    }

    public static void PlayerAnimationState(int _fromClient, Packet _packet){
        int _vAxis = _packet.ReadInt();
        int _hAxis = _packet.ReadInt();

        Server.clients[_fromClient].player.SetAxis(_vAxis, _hAxis);
    }

    public static void HelloMessage(int _fromClient, Packet _packet)
    {
        // 클라이언트로부터 받은 메시지 읽기
        string message = _packet.ReadString();
        
        if (message == "Hello World")
        {
            Console.WriteLine("Hello World");
        }
    }
}
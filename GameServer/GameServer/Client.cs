namespace GameServer;
using System;
using System.Net;
using System.Net.Sockets;
using System.Numerics;

class Client
{
    public static int dataBufferSize = 4096;

    public int id;
    public Player player;
    public TCP tcp;
    public UDP udp;
    
    public Client(int clinetId)
    {
        this.id = clinetId;
        this.tcp = new TCP(id); 
        this.udp = new UDP(id);
    }


    #region TCP Class
    public class TCP
    {
        public TcpClient socket; // 서버 요청 및 결과 처리

        private readonly int id; // 고유 ID
        private NetworkStream stream; // 소켓을 통해 데이터를 송수신하는데 사용
        private Packet receiveData;
        private byte[] receiveBuffer; // 데이터를 수신할 때 사용할 바이트 배열


        public TCP(int id){
            this.id = id;
        }

        // 클라이언트의 TCP 연결을 설정
        public void Connect(TcpClient socket)
        {
            this.socket = socket;        
            socket.ReceiveBufferSize = dataBufferSize;
            socket.SendBufferSize = dataBufferSize;

            stream = socket.GetStream(); // 데이터 전송을 수신하는데 사용되는 Network Stream 객체 생성

            receiveData = new Packet();
            receiveBuffer = new byte[dataBufferSize]; // 수신 버퍼 초기화

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null); // 비동기 읽기 작업 시작 

            ServerSend.Welcome(id, "Welcome to the server!");
        }

        public async void SendData(Packet _packet)
        {
            try
            {
                if(socket != null){
                    // 데이터를 스트림에 기록
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null); // 구버전
                    // await stream.WriteAsync(_packet.ToArray(), 0, _packet.Length()); // 신버전
                }
            }
            catch (System.Exception _ex)
            {
                System.Console.WriteLine($"Error sending data to player {id} via TCP: {_ex}");
            }
        }

        // 읽기가 완료되면 호출되는 선택적 비동기 콜백
        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result); // 실제로 읽은 바이트의 길이 확인
 
                if (_byteLength <= 0){ 
                    return; // 연결 종료
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength); // 수신한 유효한 데이터만을 _data 배열로 복사, 불필요한 데이터를 삭제
                
                receiveData.Reset(HandleData(_data));
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null); // 반복 호출, 다음 데이터 읽기
            }
            catch (System.Exception _ex)
            {
                System.Console.WriteLine($"Error: receving TCP data {_ex}");
            }
        }

        // 패킷 데이터 처리 및 분리
        private bool HandleData(byte[] _data)
        {
            int _packetLength = 0; // 패킷의 길이

            receiveData.SetBytes(_data); // 패킷을 객체로 저장

            // 패킷 길이 확인
            if (receiveData.UnreadLength() >= 4) // 최소 4byte는 읽을 수 있는 패킷이어야 함
            {
                _packetLength = receiveData.ReadInt();

                if (_packetLength <= 0) // 처리 할 데이터가 없을 경우
                {
                    return true;
                }
            }

            // 패킷 처리 루프
            while (_packetLength > 0 && _packetLength <= receiveData.UnreadLength())
            {
                byte[] _packetBytes = receiveData.ReadBytes(_packetLength); // 새로운 패킷 생성

                // 메인 스레드에서 패킷을 처리
                // using 블록 내에 객체 사용 후 패킷 클래스 메소드 Dispose() 호출 => 메모리를 해제하여 메모리 누수 방지
                ThreadManager.ExecuteOnMainThread(() => 
                {
                    using (Packet _packet = new Packet(_packetBytes)) // 새 패킷을 생성
                    {
                        int _packetId = _packet.ReadInt(); // 패킷 첫 번째 필드 ID 읽기
                        Server.packetHandlers[_packetId](id, _packet); // ID에 따라서 등록된 콜백 함수 호출
                    }
                });

                // 다음 패킷 처리 준비
                _packetLength = 0;
                if (receiveData.UnreadLength() >= 4)
                {
                    _packetLength = receiveData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            // 데이터 처리 완료 확인 
            if (_packetLength <= 1)
            {
                return true;
            }

            return false;
        }
    }
    #endregion


    #region UDP Class 
    public class UDP{
        public IPEndPoint endPoint;
        
        private int id;

        public UDP(int _id){
            id = _id;
        }
    
        public void Connect(IPEndPoint _endPoint){
            endPoint = _endPoint;
        }

        // 서버로 UDP 패킷 전송
        public void SendData(Packet _packet){
            Server.SendUDPData(endPoint, _packet);
        }

        // 서버에서 클라이언트가 보낸 UDP 데이터 처리
        public void HandleData(Packet _packetData){
            int _packetLength = _packetData.ReadInt(); // 패킷의 길이
            byte[] _packetBytes = _packetData.ReadBytes(_packetLength); // 패킷의 데이터

            ThreadManager.ExecuteOnMainThread(() => 
            {
                using(Packet _packet = new Packet(_packetBytes)){
                    int _packetId = _packet.ReadInt();
                    Server.packetHandlers[_packetId](id, _packet);
                }
            });
        }
    }
    #endregion
    

    public void SendIntoGame(string _playerName){
        player = new Player(id, _playerName, new Vector3(0, 0, 0));

        foreach (Client _client in Server.clients.Values)
        {
            if(_client.player != null){
                if(_client.id != id){
                    ServerSend.SpawnPlayer(id, _client.player);
                }
            }
        }

        foreach (Client _client in Server.clients.Values)
        {
            if(_client.player != null){
                ServerSend.SpawnPlayer(_client.id, player);
            }
        }
    }

}


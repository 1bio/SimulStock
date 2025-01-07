using System.Net;
using UnityEngine;

// 서버 -> 클라이언트 보낸 패킷 데이터 처리
public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameManager.players[_id].transform.position = _position;
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.players[_id].transform.rotation = _rotation;
    }

    public static void PlayerAnimation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int _vAxis = _packet.ReadInt();
        int _hAxis = _packet.ReadInt();

        // 플레이어 ID가 유효한지 확인
        if (GameManager.players.ContainsKey(_id))
        {
            PlayerManager playerManager = GameManager.players[_id];

            // 애니메이션 상태 업데이트
            playerManager.SetAnimationState(_vAxis, _hAxis);
        }
        else
        {
            Debug.LogWarning($"No player found with ID {_id}");
        }
    }
}
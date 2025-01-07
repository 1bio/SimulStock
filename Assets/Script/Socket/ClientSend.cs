using System;
using UnityEngine;


// 클라이언트 -> 서버 패킷 데이터 전달 
public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.usernameField.text);

            SendTCPData(_packet);
        }
    }

    public static void PlayerMovement(bool[] _inputs)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputs.Length);
            foreach (bool _input in _inputs)
            {
                _packet.Write(_input);
            }
            _packet.Write(GameManager.players[Client.instance.myId].transform.rotation);

            SendUDPData(_packet);
        }
    }

    public static void PlayerAnimation(int _vAxis, int _hAxis){
        using (Packet _packet = new Packet((int)ClientPackets.animationState)){
            _packet.Write(_vAxis);
            _packet.Write(_hAxis);
            
            SendUDPData(_packet);
        }
    }

    public static void SendHelloMessage()
    {
        using (Packet _packet = new Packet((int)ClientPackets.helloMessage))
        {
            // 메시지 보내기
            _packet.Write("Hello World");

            // UDP로 패킷 전송
            SendUDPData(_packet);
        }
    }
    #endregion
}
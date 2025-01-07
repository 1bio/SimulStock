using System;
using UnityEngine;

// 로컬 플레이어(Local Player)
public class PlayerController : MonoBehaviour
{
    public Animator animator;

    private int vAxis = 0;
    private int hAxis = 0;


    private void FixedUpdate()
    {
        SendInputToServer(); // 서버로 입력 전송
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            SendMessageToServer();
        }

        UpdateAnimationState(); // 애니메이션 상태 업데이트
    }

    // 입력을 처리하고 애니메이션 상태를 서버로 전송
    private void SendInputToServer()
    {
        bool[] inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
        };

        UpdateAxis(inputs);

        SendAnimationStateToServer(vAxis, hAxis); // 축 입력 서버로 전송

        ClientSend.PlayerMovement(inputs);
    }

    private void UpdateAxis(bool[] _inputs)
    {
        if (_inputs[0])
        {
            vAxis = 1;
        }
        else if (_inputs[1])
        {
            vAxis = -1;
        }
        else
        {
            vAxis = 0;
        }

        if (_inputs[2])
        {
            hAxis = -1;
        }
        else if (_inputs[3])
        {
            hAxis = 1;
        }
        else
        {
            hAxis = 0;
        }
    }

    #region Animation Methods
    public void UpdateAnimationState()
    {
        animator.SetInteger("vAxisRaw", vAxis);
        animator.SetInteger("hAxisRaw", hAxis);
    }

    private void SendAnimationStateToServer(int vAxis, int hAxis)
    {
        ClientSend.PlayerAnimation(vAxis, hAxis);
    }
    #endregion

    // 서버 전송 확인용 메소드 
    private void SendMessageToServer()
    {
        ClientSend.SendHelloMessage();
    }
}

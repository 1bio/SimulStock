using System;
using UnityEngine;

// 원격 플레이어(Remote Player)
public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public Animator animator;

    private int vAxisRaw = 0; // 수직 축 애니메이션 상태
    private int hAxisRaw = 0; // 수평 축 애니메이션 상태

    public void UpdateAnimationState(int vAxis, int hAxis)
    {
        vAxisRaw = vAxis;
        hAxisRaw = hAxis;

        // 애니메이션 상태 변경
        if (animator != null)
        {
            animator.SetInteger("vAxisRaw", vAxisRaw);
            animator.SetInteger("hAxisRaw", hAxisRaw);
        }
        else
        {
            Debug.LogWarning($"Animator not found on player {id}");
        }
    }

    // 추가적으로 애니메이션 상태를 외부에서 처리할 수 있도록 필요한 메소드 제공
    public void SetAnimationState(int vAxis, int hAxis)
    {
        UpdateAnimationState(vAxis, hAxis);
    }
}

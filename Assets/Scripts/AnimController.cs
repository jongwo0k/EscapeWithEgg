using UnityEngine;

// 애니메이션 통합
public class AnimController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    // 미리 실행
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // IDLE
    public void SetIDLE(bool isGround)
    {
        anim.SetBool("isGround", isGround);
    }

    // 이동 모션
    public void SetRun(bool isRun)
    {
        anim.SetBool("isRun", isRun);
    }

    // Jump/Fall 모션
    public void SetJF()
    {
        anim.SetFloat("isJF", rb.linearVelocity.y);
    }

    // 충돌 모션
    public void SetHit()
    {
        anim.SetTrigger("isHit");
    }
}
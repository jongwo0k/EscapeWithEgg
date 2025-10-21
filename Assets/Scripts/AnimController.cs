using UnityEngine;

// 애니메이션 통합
public class AnimController : MonoBehaviour
{
    private Animator anim;

    // 미리 실행
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // IDLE, Fall 종료
    public void SetGround(bool isGround)
    {
        anim.SetBool("isGround", isGround);
    }

    // 이동 모션
    public void SetRun(bool isRun)
    {
        anim.SetBool("isRun", isRun);
    }

    // Jump/Fall 모션
    public void SetJF(float yV)
    {
        anim.SetFloat("isJF", yV);
    }

    // 충돌 모션
    public void SetHit()
    {
        anim.SetTrigger("isHit");
    }

    // 획득
    public void EggGet()
    {
        anim.SetTrigger("isGet");
    }

    // 슬라이딩 (씬2만)
    public void SetSlide(bool isSlide)
    {
        anim.SetBool("isSlide", isSlide);
    }
}
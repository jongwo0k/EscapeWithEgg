using UnityEngine;

// �ִϸ��̼� ����
public class AnimController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    // �̸� ����
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

    // �̵� ���
    public void SetRun(bool isRun)
    {
        anim.SetBool("isRun", isRun);
    }

    // Jump/Fall ���
    public void SetJF()
    {
        anim.SetFloat("isJF", rb.linearVelocity.y);
    }

    // �浹 ���
    public void SetHit()
    {
        anim.SetTrigger("isHit");
    }
}
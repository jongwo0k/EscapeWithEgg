using UnityEngine;

// �ִϸ��̼� ����
public class AnimController : MonoBehaviour
{
    private Animator anim;

    // �̸� ����
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // IDLE, Fall ����
    public void SetGround(bool isGround)
    {
        anim.SetBool("isGround", isGround);
    }

    // �̵� ���
    public void SetRun(bool isRun)
    {
        anim.SetBool("isRun", isRun);
    }

    // Jump/Fall ���
    public void SetJF(float yV)
    {
        anim.SetFloat("isJF", yV);
    }

    // �浹 ���
    public void SetHit()
    {
        anim.SetTrigger("isHit");
    }

    // ȹ��
    public void EggGet()
    {
        anim.SetTrigger("isGet");
    }

    // �����̵� (��2��)
    public void SetSlide(bool isSlide)
    {
        anim.SetBool("isSlide", isSlide);
    }
}
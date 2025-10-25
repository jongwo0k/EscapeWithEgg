using UnityEngine;
using System.Collections;

public class EnemyJump : Enemy
{
    // jumpPower�� speed ��Ȱ��
    [SerializeField] private float jumpCooltime = 2f;

    // ���� ����
    private bool isGround = true;
    private Coroutine jumpCoroutine;

    protected override void Start()
    {
        base.Start();
        jumpCoroutine = StartCoroutine(JumpRoutine());
    }

    void Update()
    {
        if (isDead) return;
        anim.SetFloat("isJF", rb.linearVelocity.y);
        anim.SetBool("isGround", isGround);
    }

    IEnumerator JumpRoutine()
    {
        while (!isDead)
        {
            // ���
            isGround = true;
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            yield return new WaitForSeconds(jumpCooltime);
            if (isDead) yield break;

            // �ٰ� ���� ���� ��ȯ
            Turn(moveDirection * -1);
            yield return new WaitForSeconds(0.1f);
            if (isDead) yield break;

            // ����
            isGround = false;
            rb.linearVelocity = new Vector2(speed * moveDirection, speed);

            // ����
            yield return new WaitUntil(() => isGround && !isDead);
        }
    }

    // ���� Ȯ��
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDead && collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    } 
}

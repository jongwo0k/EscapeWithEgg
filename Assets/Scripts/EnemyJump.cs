using UnityEngine;
using System.Collections;

public class EnemyJump : Enemy
{
    // jumpPower는 speed 재활용
    [SerializeField] private float jumpCooltime = 2f;

    // 상태 변수
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
            // 대기
            isGround = true;
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            yield return new WaitForSeconds(jumpCooltime);
            if (isDead) yield break;

            // 뛰고 나면 방향 전환
            Turn(moveDirection * -1);
            yield return new WaitForSeconds(0.1f);
            if (isDead) yield break;

            // 점프
            isGround = false;
            rb.linearVelocity = new Vector2(speed * moveDirection, speed);

            // 착지
            yield return new WaitUntil(() => isGround && !isDead);
        }
    }

    // 착지 확인
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDead && collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    } 
}

using UnityEngine;

public class EnemyWork : Enemy
{
    // Enemy의 Start가 먼저 실행
    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        // 사망
        if (isDead) return;

        // 방향 전환
        if (transform.position.x >= maxX)
        {
            Turn(-1);
        }
        else if (transform.position.x <= minX)
        {
            Turn(1);
        }
    }

    // 시간 간격 고정
    void FixedUpdate()
    {
        // 순찰
        rb.linearVelocity = new Vector2(speed * moveDirection, rb.linearVelocity.y);
    }
}

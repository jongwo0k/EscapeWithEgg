using UnityEngine;

public class EnemyWork : Enemy
{
    // Enemy�� Start�� ���� ����
    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        // ���
        if (isDead) return;

        // ���� ��ȯ
        if (transform.position.x >= maxX)
        {
            Turn(-1);
        }
        else if (transform.position.x <= minX)
        {
            Turn(1);
        }
    }

    // �ð� ���� ����
    void FixedUpdate()
    {
        // ����
        rb.linearVelocity = new Vector2(speed * moveDirection, rb.linearVelocity.y);
    }
}

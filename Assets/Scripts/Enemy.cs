using UnityEngine;

public class Enemy : MonoBehaviour
{
    // �̵� ����
    [SerializeField] protected int HP = 1;
    protected bool isDead = false;
    protected int moveDirection = 1;

    [SerializeField] protected bool startDirection = false;
    [SerializeField] protected float speed = 3f;
    [SerializeField] protected float minX;
    [SerializeField] protected float maxX;

    protected Animator anim;
    protected Collider2D col;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        // ���� ����
        if (startDirection)
        {
            moveDirection = -1;
            sr.flipX = false;
        }
        else
        {
            moveDirection = 1;
            sr.flipX = false;
        }
    }

    // ����
    public virtual void EnemyHit()
    {
        HP--;

        if (HP > 0)
        {
            anim.SetTrigger("isBear"); // Bear�� ���� (����)
        }

        else
        {
            anim.SetTrigger("isHit");

            isDead = true;
            rb.linearVelocity = Vector2.zero;
            col.enabled = false;

            // �Ӹ� �κ� emptyObject
            Collider2D Enemy = transform.Find("EnemyHead")?.GetComponent<Collider2D>();
            if (Enemy != null)
            {
                Enemy.enabled = false;
            }
        }
    }

    // ��� ��� �� �ı�
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    // ���� ��ȯ
    protected void Turn(int direction)
    {
        moveDirection = direction;
        if (startDirection)
        {
            sr.flipX = (moveDirection == 1);
        }
        else
        {
            sr.flipX = (moveDirection == -1);
        }
    }
}
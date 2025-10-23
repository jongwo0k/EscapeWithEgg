using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 이동 변수
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

        // 방향 통일
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

    // 밟힘
    public virtual void EnemyHit()
    {
        HP--;

        if (HP > 0)
        {
            anim.SetTrigger("isBear"); // Bear만 있음 (주의)
        }

        else
        {
            anim.SetTrigger("isHit");

            isDead = true;
            rb.linearVelocity = Vector2.zero;
            col.enabled = false;

            // 머리 부분 emptyObject
            Collider2D Enemy = transform.Find("EnemyHead")?.GetComponent<Collider2D>();
            if (Enemy != null)
            {
                Enemy.enabled = false;
            }
        }
    }

    // 사망 모션 후 파괴
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    // 뱡향 전환
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
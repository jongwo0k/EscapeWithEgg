/*using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 이동 변수
    public int HP = 1;
    public float speed = 5f;
    private int moveDirection = 1;

    // 애니메이션, 충돌
    private AnimController ac;
    private SpriteRenderer sr;
    private Collider2D col;
    private bool isDead = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        ac = GetComponent<AnimController>();
    }

    void Update()
    {
        // 밟힘
        if (isDead) return;

        // 순찰
        transform.Translate(Vector2.right * speed * moveDirection * Time.deltaTime); // 프레임 상관없이 속도 고정

        // 방향 전환
        if (transform.position.x >= maxX)
        {
            moveDirection = -1;
            sr.flipX = true;
        }
        else if (transform.position.x <= minX)
        {
            moveDirection = 1;
            sr.flipX = false;

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            // 부딪히면 데미지를 줌, 머리를 밟히면 사망
            if (collision.contacts[0].normal.y < -0.7f)
            {
                isDead = true;
                col.enabled = false; // 사망 모션 중 충돌 방지
                anim.SetTrigger("isDie");
            }
            else
            {
                collision.gameObject.GetComponent<Player>().CollisionOb();
            }
        }

    }

    // 사망 모션 후 파괴
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
*/
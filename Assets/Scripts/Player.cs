using UnityEngine;

public class Player : MonoBehaviour
{
    // 캐릭터 이동 변수
    public float speed = 3f;
    public float jumpPower = 6f;
    private bool isGround;

    /* 아직
    public int HP = 3;
    private int MaxHP;

    public GameObject Hit_Prefab;
    public Slider HpBar;
    */

    // 애니메이션
    private AnimController ac;

    // 물리 엔진
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ac = GetComponent<AnimController>();

        // Player 시작 위치 지정
        transform.position = new Vector3(-10f, -0.15f, 0f);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
            sr.flipX = false;
            ac.SetRun(isGround);

        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
            sr.flipX = true;
            ac.SetRun(isGround);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            ac.SetRun(false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        }

        ac.SetJF(rb.linearVelocity.y);
        ac.SetGround(isGround);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            // 데미지 피격 함수
            // TakeDamage();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 땅에서 떨어졌을 때
            isGround = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Target")
        {
            // 계란 획득 -> 보스 등장 컷씬 후 씬 이동
        }
    }

    /* 피격 (아직)
    private void TakeDamage()
    {
        HP--;
    }
    */
}

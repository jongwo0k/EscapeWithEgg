using UnityEngine;

public class Player2 : MonoBehaviour
{
    // 캐릭터 이동 변수
    public float speed = 3f;
    public float jumpPower = 5f;
    public int maxJumpCount = 2;
    private int jumpCount = 0;
    bool isGround;

    // 캐릭터 상태 변수
    bool isOver = false;

    // 애니메이션
    private AnimController ac;

    // 물리 엔진
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    // UI
    public GameObject Hit_Prefab;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ac = GetComponent<AnimController>();

        // UI 초기화
        UI_Manager.Instance.StartTimer();

        // Player 시작 위치 지정
        transform.position = new Vector3(-1f, -0.199f, 0f);
    }

    void Update()
    {
        if (isOver)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        ac.SetJF(rb.linearVelocity.y);
        ac.SetGround(isGround);

        // 더블 점프
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumpCount)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            jumpCount++;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Jump
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            jumpCount = 0;
        }

        // 충돌
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Die");
            Die();// 현재 테스트 중
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 땅에서 떨어졌을 때 (Jump)
            isGround = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Clear
        if (collision.gameObject.CompareTag("End"))
        {
            UI_Manager.Instance.GameIsClear();
        }

        // 추락
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            Die();
        }
    }

    // 사망
    public void Die()
    {
        if (isOver) return;
        isOver = true;

        // Particle
        GameObject go = Instantiate(Hit_Prefab, transform.position, Quaternion.identity);
        Destroy(go, 1.0f);
        ac.SetHit();

        // 정지
        rb.linearVelocity = Vector2.zero;
        UI_Manager.Instance.GameIsOver();
        Destroy(gameObject);
    }
}
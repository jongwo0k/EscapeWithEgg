using UnityEngine;

public class Player2 : MonoBehaviour
{
    // ĳ���� �̵� ����
    public float speed = 3f;
    public float jumpPower = 5f;
    public int maxJumpCount = 2;
    private int jumpCount = 0;
    bool isGround;

    // ĳ���� ���� ����
    bool isOver = false;

    // �ִϸ��̼�
    private AnimController ac;

    // ���� ����
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    // UI
    public GameObject Hit_Prefab;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ac = GetComponent<AnimController>();

        // UI �ʱ�ȭ
        UI_Manager.Instance.StartTimer();

        // Player ���� ��ġ ����
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

        // ���� ����
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

        // �浹
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Die");
            Die();// ���� �׽�Ʈ ��
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // ������ �������� �� (Jump)
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

        // �߶�
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            Die();
        }
    }

    // ���
    public void Die()
    {
        if (isOver) return;
        isOver = true;

        // Particle
        GameObject go = Instantiate(Hit_Prefab, transform.position, Quaternion.identity);
        Destroy(go, 1.0f);
        ac.SetHit();

        // ����
        rb.linearVelocity = Vector2.zero;
        UI_Manager.Instance.GameIsOver();
        Destroy(gameObject);
    }
}
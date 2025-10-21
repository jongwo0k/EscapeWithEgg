using UnityEngine;

public class Player : MonoBehaviour
{
    // ĳ���� �̵� ����
    public float speed = 3f;
    public float jumpPower = 6f;
    private bool isGround;

    /* ����
    public int HP = 3;
    private int MaxHP;

    public GameObject Hit_Prefab;
    public Slider HpBar;
    */

    // �ִϸ��̼�
    private AnimController ac;

    // ���� ����
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ac = GetComponent<AnimController>();

        // Player ���� ��ġ ����
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
            // ������ �ǰ� �Լ�
            // TakeDamage();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // ������ �������� ��
            isGround = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Target")
        {
            // ��� ȹ�� -> ���� ���� �ƾ� �� �� �̵�
        }
    }

    /* �ǰ� (����)
    private void TakeDamage()
    {
        HP--;
    }
    */
}

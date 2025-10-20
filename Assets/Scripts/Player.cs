using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public float speed = 3f;
    public float jumpPower = 5f;

    public int HP = 3;
    private int MaxHP;

    public GameObject Hit_Prefab;
    public Slider HpBar;

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

        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
            sr.flipX = true;


        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);


        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);

            // AnimatorChange("isJump");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // 데미지 피격 함수
            TakeDamage();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Target")
        {
            // 계란 획득 -> 보스 등장 컷씬 후 씬 이동
        }
    }

    // 피격
    private void TakeDamage()
    {
        HP--;
    }
}

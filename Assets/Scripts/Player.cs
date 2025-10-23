using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    // 캐릭터 이동 변수
    public float speed = 3f;
    public float jumpPower = 6f;
    private bool isGround;

    // 캐릭터 상태 변수
    public int HP = 3;
    private int MaxHP;
    private bool isInvincible;
    bool isOver = false;

    public GameObject Hit_Prefab;

    // 애니메이션
    private AnimController ac;

    // 물리 엔진
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    // UI
    private UI_Manager um;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ac = GetComponent<AnimController>();

        // UI 초기화
        MaxHP = HP;
        UI_Manager.Instance.HP_Update(HP, MaxHP);

        // Player 시작 위치 지정
        transform.position = new Vector3(-10f, -0.15f, 0f);
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

        // 정면 넉백
        if (isInvincible)
        {
            ac.SetRun(false);
            return;
        }

        // 방향키로 이동
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Jump
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }

        // Enemy 공격
        if (collision.collider.CompareTag("EnemyHead"))
        {
            rb.linearVelocity = Vector2.zero;
            Vector2 knockbackDirection = collision.contacts[0].normal;
            rb.AddForce(knockbackDirection * 7f, ForceMode2D.Impulse);

            Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.EnemyHit();
            }
        }

        // 체력 감소
        if (collision.collider.CompareTag("Enemy"))
        {
            TakeDamage(collision);
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
        if (collision.gameObject.tag == "Target") // Egg
        {
            // 계란 획득 -> 보스 등장 컷씬


            // 이후 씬 이동
            UI_Manager.Instance.nextScene();
        }

        // Clear
        if (collision.gameObject.tag == "End")
        {
            UI_Manager.Instance.GameIsClear();
        }

        // 추락
        if (collision.gameObject.tag == "DeadZone")
        {
            Die();
        }
    }

    // 데미지 처리
    private void TakeDamage(Collision2D collision)
    {
        if (isInvincible) return;
        StartCoroutine(InvincibilityCoroutine(collision));
    }

    // 애니메이션이 재생되는 동안은 무적 상태로 대기
    private IEnumerator InvincibilityCoroutine(Collision2D collision)
    {
        isInvincible = true;

        // 체력 상태 관리
        HP--;
        UI_Manager.Instance.HP_Update(HP, MaxHP);

        // Particle
        GameObject go = Instantiate(Hit_Prefab, transform.position, Quaternion.identity);
        Destroy(go, 1.0f);
        ac.SetHit();

        // 충돌 시 밀려남
        rb.linearVelocity = Vector2.zero;
        Vector2 knockbackDirection = collision.contacts[0].normal;
        rb.AddForce(knockbackDirection * 5f, ForceMode2D.Impulse);

        if (HP <= 0)
        {
            Die();
            yield break;
        }

        // 중복 충돌 방지 (잠시 무적)
        yield return new WaitForSeconds(0.7f); // 애니메이션 재생시간과 통일(0.7f)
        isInvincible = false;
    }

    // 사망
    public void Die()
    {
        if (isOver) return;
        isOver = true;

        // 추락, 충돌 공용
        HP = 0;
        UI_Manager.Instance.HP_Update(HP, MaxHP);

        // 정지
        rb.linearVelocity = Vector2.zero;
        ac.SetRun(false);

        UI_Manager.Instance.GameIsOver();
        Destroy(gameObject);
    }
}
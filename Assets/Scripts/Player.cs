using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    // ĳ���� �̵� ����
    public float speed = 3f;
    public float jumpPower = 6f;
    private bool isGround;

    // ĳ���� ���� ����
    public int HP = 3;
    private int MaxHP;
    private bool isInvincible;
    bool isOver = false;

    public GameObject Hit_Prefab;

    // �ִϸ��̼�
    private AnimController ac;

    // ���� ����
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    // UI
    private UI_Manager um;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ac = GetComponent<AnimController>();

        // UI �ʱ�ȭ
        MaxHP = HP;
        UI_Manager.Instance.HP_Update(HP, MaxHP);

        // Player ���� ��ġ ����
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

        // ���� �˹�
        if (isInvincible)
        {
            ac.SetRun(false);
            return;
        }

        // ����Ű�� �̵�
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

        // Enemy ����
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

        // ü�� ����
        if (collision.collider.CompareTag("Enemy"))
        {
            TakeDamage(collision);
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
        if (collision.gameObject.tag == "Target") // Egg
        {
            // ��� ȹ�� -> ���� ���� �ƾ�


            // ���� �� �̵�
            UI_Manager.Instance.nextScene();
        }

        // Clear
        if (collision.gameObject.tag == "End")
        {
            UI_Manager.Instance.GameIsClear();
        }

        // �߶�
        if (collision.gameObject.tag == "DeadZone")
        {
            Die();
        }
    }

    // ������ ó��
    private void TakeDamage(Collision2D collision)
    {
        if (isInvincible) return;
        StartCoroutine(InvincibilityCoroutine(collision));
    }

    // �ִϸ��̼��� ����Ǵ� ������ ���� ���·� ���
    private IEnumerator InvincibilityCoroutine(Collision2D collision)
    {
        isInvincible = true;

        // ü�� ���� ����
        HP--;
        UI_Manager.Instance.HP_Update(HP, MaxHP);

        // Particle
        GameObject go = Instantiate(Hit_Prefab, transform.position, Quaternion.identity);
        Destroy(go, 1.0f);
        ac.SetHit();

        // �浹 �� �з���
        rb.linearVelocity = Vector2.zero;
        Vector2 knockbackDirection = collision.contacts[0].normal;
        rb.AddForce(knockbackDirection * 5f, ForceMode2D.Impulse);

        if (HP <= 0)
        {
            Die();
            yield break;
        }

        // �ߺ� �浹 ���� (��� ����)
        yield return new WaitForSeconds(0.7f); // �ִϸ��̼� ����ð��� ����(0.7f)
        isInvincible = false;
    }

    // ���
    public void Die()
    {
        if (isOver) return;
        isOver = true;

        // �߶�, �浹 ����
        HP = 0;
        UI_Manager.Instance.HP_Update(HP, MaxHP);

        // ����
        rb.linearVelocity = Vector2.zero;
        ac.SetRun(false);

        UI_Manager.Instance.GameIsOver();
        Destroy(gameObject);
    }
}
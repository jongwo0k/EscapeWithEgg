using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    // ĳ���� �̵� ����
    public float speed = 3f;
    public float jumpPower = 6f;
    bool isGround;

    // ĳ���� ���� ����
    public int HP = 3;
    private int MaxHP;
    private bool isInvincible;
    bool isOver = false;
    bool getEgg = false;

    // Cutscene
    [SerializeField] private CameraController CameraController;
    [SerializeField] private GameObject boss;
    [SerializeField] private Vector3 bossScale = new Vector3(5f, 5f, 5f);
    private float cutsceneDuration = 4f;
    private float cameraDuration = 1f;

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

        if (!getEgg)
        {
            // ����Ű�� �̵�
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
                sr.flipX = false;
                ac.SetRun(isGround);

            }
            else if (Input.GetKey(KeyCode.LeftArrow))
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Jump
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }

        // ��Ƽ� ���� ����
        if (collision.collider.CompareTag("EnemyHead"))
        {
            // Ƣ�� ����
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
        if (collision.gameObject.CompareTag("Target")) // Egg
        {
            // ���� ����� ��� �� �������� �̵�
            rb.linearVelocity = Vector2.zero;
            getEgg = true;
            // (ȹ�� ȿ�� �߰�?)
            StartCoroutine(StartBossCutscene());
        }

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

    // �ƽ� �ڷ�ƾ
    IEnumerator StartBossCutscene()
    {
        // Player���� Boss�� ���� ��ȯ
        if (CameraController != null)
        {
            CameraController.enabled = false;
        }
        
        // ũ��, ��ġ ����
        Camera mainCamera = Camera.main;
        Vector3 playerPositionAtEggGet = transform.position;
        Vector3 bossStartPosition = boss.transform.position;
        Vector3 bossStartScale = boss.transform.localScale;
        Vector3 cameraStartPosition = mainCamera.transform.position;
        Vector3 cameraEndPosition = new Vector3(bossStartPosition.x, bossStartPosition.y, cameraStartPosition.z);

        // ���� �������� Player���� ���ư�
        float passedTime = 0f;
        while (passedTime < cameraDuration)
        {
            mainCamera.transform.position = Vector3.Lerp(cameraStartPosition, cameraEndPosition, passedTime / cameraDuration);
            passedTime += Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.position = cameraEndPosition;

        // ī�޶� ��Ʈ�ѷ��� Ÿ���� ������ ����
        if (CameraController != null)
        {
            CameraController.target = boss.transform;
            CameraController.enabled = true;
        }

        // �ٰ����鼭 Ŀ��
        boss.SetActive(true);
        float bossAnimDuration = cutsceneDuration - cameraDuration;
        passedTime = 0f;
        while (passedTime < bossAnimDuration)
        {
            float t = passedTime / bossAnimDuration;
            boss.transform.localScale = Vector3.Lerp(bossStartScale, bossScale, t);
            boss.transform.position = Vector3.Lerp(bossStartPosition, playerPositionAtEggGet, t);
            passedTime += Time.deltaTime;
            yield return null;
        }

        // �����ϸ� ���� ������
        UI_Manager.Instance.nextScene();
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
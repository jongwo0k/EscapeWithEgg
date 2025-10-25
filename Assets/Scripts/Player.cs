using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    // 캐릭터 이동 변수
    public float speed = 3f;
    public float jumpPower = 6f;
    bool isGround;

    // 캐릭터 상태 변수
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

        if (!getEgg)
        {
            // 방향키로 이동
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

        // 밞아서 적을 공격
        if (collision.collider.CompareTag("EnemyHead"))
        {
            // 튀어 오름
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
        if (collision.gameObject.CompareTag("Target")) // Egg
        {
            // 보스 등장씬 재생 후 스테이지 이동
            rb.linearVelocity = Vector2.zero;
            getEgg = true;
            // (획득 효과 추가?)
            StartCoroutine(StartBossCutscene());
        }

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

    // 컷신 코루틴
    IEnumerator StartBossCutscene()
    {
        // Player에서 Boss로 시점 전환
        if (CameraController != null)
        {
            CameraController.enabled = false;
        }
        
        // 크기, 위치 조절
        Camera mainCamera = Camera.main;
        Vector3 playerPositionAtEggGet = transform.position;
        Vector3 bossStartPosition = boss.transform.position;
        Vector3 bossStartScale = boss.transform.localScale;
        Vector3 cameraStartPosition = mainCamera.transform.position;
        Vector3 cameraEndPosition = new Vector3(bossStartPosition.x, bossStartPosition.y, cameraStartPosition.z);

        // 보스 시점에서 Player에게 날아감
        float passedTime = 0f;
        while (passedTime < cameraDuration)
        {
            mainCamera.transform.position = Vector3.Lerp(cameraStartPosition, cameraEndPosition, passedTime / cameraDuration);
            passedTime += Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.position = cameraEndPosition;

        // 카메라 컨트롤러의 타겟을 보스로 변경
        if (CameraController != null)
        {
            CameraController.target = boss.transform;
            CameraController.enabled = true;
        }

        // 다가가면서 커짐
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

        // 도착하면 다음 씬으로
        UI_Manager.Instance.nextScene();
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
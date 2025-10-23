/*using UnityEngine;

public class Enemy : MonoBehaviour
{
    // �̵� ����
    public int HP = 1;
    public float speed = 5f;
    private int moveDirection = 1;

    // �ִϸ��̼�, �浹
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
        // ����
        if (isDead) return;

        // ����
        transform.Translate(Vector2.right * speed * moveDirection * Time.deltaTime); // ������ ������� �ӵ� ����

        // ���� ��ȯ
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
            // �ε����� �������� ��, �Ӹ��� ������ ���
            if (collision.contacts[0].normal.y < -0.7f)
            {
                isDead = true;
                col.enabled = false; // ��� ��� �� �浹 ����
                anim.SetTrigger("isDie");
            }
            else
            {
                collision.gameObject.GetComponent<Player>().CollisionOb();
            }
        }

    }

    // ��� ��� �� �ı�
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
*/
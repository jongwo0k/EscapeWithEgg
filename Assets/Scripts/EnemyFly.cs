using UnityEngine;

public class EnemyFly : Enemy
{
    protected override void Start()
    {
        base.Start();
    }

    // Kinematic, ÁÂÇ¥ ±â¹Ý
    void Update()
    {
        if (isDead) return;

        transform.Translate(Vector2.right * speed * moveDirection * Time.deltaTime);

        if (transform.position.x >= maxX)
        {
            Turn(-1);
        }
        else if (transform.position.x <= minX)
        {
            Turn(1);
        }
    }
}
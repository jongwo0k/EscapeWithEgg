using UnityEngine;

public class MapMover : MonoBehaviour
{
    public float mapSpeed;
    public float destroyX = -30f;
    public MapSpawner MapSpawner;

    void Update()
    {
        if (UI_Manager.Instance.isOver) return;

        // 속도에 맞춰서 이동
        float currentSpeed = MapSpawner != null ? MapSpawner.CurrentSpeed() : mapSpeed;
        transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);

        // 범위 밖으로 나가면 삭제
        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}
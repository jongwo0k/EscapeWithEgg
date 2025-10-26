using UnityEngine;

public class MapMover : MonoBehaviour
{
    public float mapSpeed;
    public float destroyX = -30f;
    public MapSpawner MapSpawner;

    void Update()
    {
        if (UI_Manager.Instance.isOver) return;

        // �ӵ��� ���缭 �̵�
        float currentSpeed = MapSpawner != null ? MapSpawner.CurrentSpeed() : mapSpeed;
        transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);

        // ���� ������ ������ ����
        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}
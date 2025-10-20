using UnityEngine;

public class CameraController : MonoBehaviour
{
    // ī�޶� ����
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    // �� ����
    public float maxX = 20f;
    public float minX = -2.5f;
    public float maxY = 13f;
    public float minY = 0.5f;

    // �������� ����
    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // ȭ�� ��Ż ����
        smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minX, maxX);
        smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minY, maxY);

        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
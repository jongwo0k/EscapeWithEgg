using UnityEngine;
using System.Collections;

public class MapSpawner : MonoBehaviour
{
    // Ÿ�� ���� ����
    public float width = 22f;
    public float moveSpeed = 5f;
    public float maxSpeed = 12f;
    public float speedIncreaseRate = 0.05f;
    public float spawnInterval = 5f;
    private float firstInterval = 2f;
    private float nextSpawnX = 0;
    private bool finalSpawned = false;

    // Ÿ�� ������
    [SerializeField] private GameObject startTile;
    [SerializeField] private GameObject[] Tiles;
    [SerializeField] private GameObject finalTile;

    void Start()
    {
        SpawnTile(startTile);
        InvokeRepeating("SpawnNextTile", firstInterval, spawnInterval);
    }

    void Update()
    {
        // ���� �ӵ� ����
        if (moveSpeed < maxSpeed)
        {
            moveSpeed += speedIncreaseRate * Time.deltaTime;
        }
    }

    // ���� Ÿ�� ����
    void SpawnNextTile()
    {
        if (finalSpawned) return;

        SpawnRandomTile();
    }

    // Ÿ�� ���� ����
    void SpawnRandomTile()
    {
        int idx = Random.Range(0, Tiles.Length);
        SpawnTile(Tiles[idx]);
    }

    // Ÿ�� ����
    void SpawnTile(GameObject tilePrefab)
    {
        Vector3 spawnPos = new Vector3(nextSpawnX, 0, 0);
        GameObject tile = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);

        // ĳ���ʹ� ����, ���� �̵�
        MapMover mover = tile.AddComponent<MapMover>();
        mover.mapSpeed = moveSpeed;
        mover.MapSpawner = this;

        // ���� Ÿ�� ��ġ
        nextSpawnX += width;
    }

    // Ÿ�̸� ����
    public void SpawnFinalTile()
    {
        // �Ϲ� Ÿ�� ���� ����, ������ Ÿ�� ����
        finalSpawned = true;
        CancelInvoke("SpawnNextTile");
        SpawnTile(finalTile);
    }

    // ���� �̵� �ӵ�
    public float CurrentSpeed()
    {
        return moveSpeed;
    }
}
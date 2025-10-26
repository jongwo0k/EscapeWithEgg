using UnityEngine;
using System.Collections;

public class MapSpawner : MonoBehaviour
{
    // 타일 상태 변수
    public float width = 22f;
    public float moveSpeed = 5f;
    public float maxSpeed = 12f;
    public float speedIncreaseRate = 0.05f;
    public float spawnInterval = 5f;
    private float firstInterval = 2f;
    private float nextSpawnX = 0;
    private bool finalSpawned = false;

    // 타일 프리팹
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
        // 점점 속도 증가
        if (moveSpeed < maxSpeed)
        {
            moveSpeed += speedIncreaseRate * Time.deltaTime;
        }
    }

    // 다음 타일 생성
    void SpawnNextTile()
    {
        if (finalSpawned) return;

        SpawnRandomTile();
    }

    // 타일 랜덤 생성
    void SpawnRandomTile()
    {
        int idx = Random.Range(0, Tiles.Length);
        SpawnTile(Tiles[idx]);
    }

    // 타일 생성
    void SpawnTile(GameObject tilePrefab)
    {
        Vector3 spawnPos = new Vector3(nextSpawnX, 0, 0);
        GameObject tile = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);

        // 캐릭터는 고정, 맵이 이동
        MapMover mover = tile.AddComponent<MapMover>();
        mover.mapSpeed = moveSpeed;
        mover.MapSpawner = this;

        // 다음 타일 위치
        nextSpawnX += width;
    }

    // 타이머 종료
    public void SpawnFinalTile()
    {
        // 일반 타일 생성 중지, 마지막 타일 생성
        finalSpawned = true;
        CancelInvoke("SpawnNextTile");
        SpawnTile(finalTile);
    }

    // 현재 이동 속도
    public float CurrentSpeed()
    {
        return moveSpeed;
    }
}
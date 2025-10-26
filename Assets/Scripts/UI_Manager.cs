using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    // singleton
    public static UI_Manager Instance { get; private set; }
    public bool isOver = false;

    // 체력 관리
    public TextMeshProUGUI HP_Text;
    public Slider slider;

    // 타이머 관리
    public TextMeshProUGUI Time_Text;
    public float survivalTime = 60f;
    private float remainTime;
    private bool isTimerRunning = false;
    private bool finalTileSpawned = false;

    // Canvas 관리
    public GameObject gameOver;
    public GameObject gameClear;

    // 초기 설정
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // 타이머
    private void Update()
    {
        // 씬2에서만 사용
        if (isTimerRunning)
        {
            remainTime -= Time.deltaTime;

            if (remainTime <= 0f && !finalTileSpawned)
            {
                // 상태 고정
                remainTime = 0f;
                isTimerRunning = false;
                finalTileSpawned = true;
                Time_Text.text = "Time: " + remainTime.ToString("F1");

                // 엔딩 지점 생성
                MapSpawner MapSpawn = FindFirstObjectByType<MapSpawner>();
                MapSpawn.SpawnFinalTile();
            }
            Time_Text.text = "Time: " + remainTime.ToString("F1");
        }
    }

    // 체력 표시 (씬1에서만 사용)
    public void HP_Update(int HP, int MaxHP)
    {
        HP_Text.text = "HP : " + HP.ToString();
        slider.value = (float)HP / MaxHP;
    }

    // 타이머 시작
    public void StartTimer()
    {
        remainTime = survivalTime;
        isTimerRunning = true;
        finalTileSpawned = false;
        Time_Text.gameObject.SetActive(true);
    }

    // 재시작 (씬1부터)
    public void Retry_Button()
    {
        SceneManager.LoadScene("FirstScene");
    }

    // 1단계 clear -> 2단계로
    public void nextScene()
    {
        SceneManager.LoadScene("SecondScene");
    }

    // GameOver
    public void GameIsOver()
    {
        isTimerRunning = false;
        gameOver.SetActive(true);
    }

    // GameClear
    public void GameIsClear()
    {
        gameClear.SetActive(true);
    }
}
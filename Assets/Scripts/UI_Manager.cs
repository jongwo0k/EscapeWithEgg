using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    // singleton
    public static UI_Manager Instance { get; private set; }
    public bool isOver = false;

    // ü�� ����
    public TextMeshProUGUI HP_Text;
    public Slider slider;

    // Ÿ�̸� ����
    public TextMeshProUGUI Time_Text;
    public float survivalTime = 60f;
    private float remainTime;
    private bool isTimerRunning = false;
    private bool finalTileSpawned = false;

    // Canvas ����
    public GameObject gameOver;
    public GameObject gameClear;

    // �ʱ� ����
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

    // Ÿ�̸�
    private void Update()
    {
        // ��2������ ���
        if (isTimerRunning)
        {
            remainTime -= Time.deltaTime;

            if (remainTime <= 0f && !finalTileSpawned)
            {
                // ���� ����
                remainTime = 0f;
                isTimerRunning = false;
                finalTileSpawned = true;
                Time_Text.text = "Time: " + remainTime.ToString("F1");

                // ���� ���� ����
                MapSpawner MapSpawn = FindFirstObjectByType<MapSpawner>();
                MapSpawn.SpawnFinalTile();
            }
            Time_Text.text = "Time: " + remainTime.ToString("F1");
        }
    }

    // ü�� ǥ�� (��1������ ���)
    public void HP_Update(int HP, int MaxHP)
    {
        HP_Text.text = "HP : " + HP.ToString();
        slider.value = (float)HP / MaxHP;
    }

    // Ÿ�̸� ����
    public void StartTimer()
    {
        remainTime = survivalTime;
        isTimerRunning = true;
        finalTileSpawned = false;
        Time_Text.gameObject.SetActive(true);
    }

    // ����� (��1����)
    public void Retry_Button()
    {
        SceneManager.LoadScene("FirstScene");
    }

    // 1�ܰ� clear -> 2�ܰ��
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
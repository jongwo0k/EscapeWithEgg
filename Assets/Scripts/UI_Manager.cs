using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    // singleton
    public static UI_Manager Instance { get; private set; }

    // 체력 관리
    public TextMeshProUGUI HP_Text;
    public Slider slider;
    private int MaxHP;

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

    // 체력 표시
    public void HP_Update(int HP, int MaxHP)
    {
        HP_Text.text = "HP : " + HP.ToString();
        slider.value = (float)HP / MaxHP;
    }

    // 재시작
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
        gameOver.SetActive(true);
    }

    // GameClear
    public void GameIsClear()
    {
        gameClear.SetActive(true);
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    // singleton
    public static UI_Manager Instance { get; private set; }

    // ü�� ����
    public TextMeshProUGUI HP_Text;
    public Slider slider;
    private int MaxHP;

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

    // ü�� ǥ��
    public void HP_Update(int HP, int MaxHP)
    {
        HP_Text.text = "HP : " + HP.ToString();
        slider.value = (float)HP / MaxHP;
    }

    // �����
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
        gameOver.SetActive(true);
    }

    // GameClear
    public void GameIsClear()
    {
        gameClear.SetActive(true);
    }
}

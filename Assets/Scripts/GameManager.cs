using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject clearPanel;
    public TextMeshProUGUI playTimeText_UI;
    public TextMeshProUGUI playTimeText_Clear;
    public Button retryButton;

    private float playTime;
    private bool isCleared = false;
    private string measureTime;

    void Update()
    {
        if (!isCleared)
        {
            playTime += Time.deltaTime;
            UpdatePlayTimeText();
        }
    }

    void UpdatePlayTimeText()
    {
        int totalSeconds = (int)playTime;

        // 최대치 제한: 99시간 59분 59초(359999초)
        if (totalSeconds > 359999)
            totalSeconds = 359999;

        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        measureTime = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        playTimeText_UI.text = measureTime;
    }

    public void TriggerClear()
    {
        isCleared = true;
        clearPanel.SetActive(true);
        playTimeText_Clear.text = measureTime;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
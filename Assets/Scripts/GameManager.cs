using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject clearPanel;
    public TextMeshProUGUI playTimeText_UI;
    public TextMeshProUGUI playTimeText_Clear;

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

        playTimeText_UI.text = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        measureTime = $"{hours / 10} {hours % 10}  :  {minutes / 10} {minutes % 10}  :  {seconds / 10} {seconds % 10}";
    }

    public void TriggerClear()
    {
        isCleared = true;
        clearPanel.SetActive(true);
        playTimeText_Clear.text = measureTime;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToHome()
    {
        SceneManager.LoadScene("MainUI");
    }
}
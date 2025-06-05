using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIManager : MonoBehaviour
{
    public GameObject itemNoticePanel;
    public GameObject gameNoticePanel;

    // --- ������ ���� �г� ---
    public void ShowItemNoticePanel()
    {
        if (itemNoticePanel != null)
            itemNoticePanel.SetActive(true);
    }

    public void HideItemNoticePanel()
    {
        if (itemNoticePanel != null)
            itemNoticePanel.SetActive(false);
    }

    // --- ���� ���� �г� ---
    public void ShowGameNoticePanel()
    {
        if (gameNoticePanel != null)
            gameNoticePanel.SetActive(true);
    }

    public void HideGameNoticePanel()
    {
        if (gameNoticePanel != null)
            gameNoticePanel.SetActive(false);
    }

    // --- ���� �޴� ��� ---
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
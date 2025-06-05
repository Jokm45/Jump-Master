using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIManager : MonoBehaviour
{
    public GameObject itemNoticePanel;
    public GameObject gameNoticePanel;

    // --- 아이템 설명 패널 ---
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

    // --- 게임 설명 패널 ---
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

    // --- 메인 메뉴 기능 ---
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
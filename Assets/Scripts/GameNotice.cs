using UnityEngine;

public class GameNotice : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject GameHelpPanel;

    public void ShowGameHelpPanel()
    {
        GameHelpPanel.SetActive(true);
    }

    public void HideGameHelpPanel()
    {
        GameHelpPanel.SetActive(false);
    }
}

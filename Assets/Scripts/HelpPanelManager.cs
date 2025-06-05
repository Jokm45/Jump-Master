using UnityEngine;

public class HelpPanelManager : MonoBehaviour
{
    public GameObject helpPanel;

    public void ShowHelpPanel()
    {
        helpPanel.SetActive(true);
    }

    public void HideHelpPanel()
    {
        helpPanel.SetActive(false);
    }
}

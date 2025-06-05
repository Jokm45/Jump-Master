using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene"); // 불러올 씬 이름
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject clearPanel;
    public TextMeshProUGUI playTimeText_UI;
    public TextMeshProUGUI playTimeText_Clear;
    public Transform playerTransform;

    private float playTime;
    private bool isCleared = false;
    private string measureTime;

    void Start()
    {
        // �÷��� Ÿ�� �ҷ�����
        playTime = PlayerPrefs.GetFloat("PlayTime", 0f);

        // �÷��̾� ��ġ �ҷ�����
        LoadPlayerPosition(playerTransform);
    }

    void Update()
    {
        // I Ű�� ������ �ʱ�ȭ �� �� �����
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartFromBeginning();
            return;
        }

        if (!isCleared)
        {
            playTime += Time.deltaTime;
            UpdatePlayTimeText();

            // �÷��� Ÿ�Ӱ� ��ġ ����
            PlayerPrefs.SetFloat("PlayTime", playTime);
            SavePlayerPosition(playerTransform);
        }
    }

    void UpdatePlayTimeText()
    {
        int totalSeconds = (int)playTime;

        // �ִ�ġ ����: 99�ð� 59�� 59��(359999��)
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
        PlayerPrefs.DeleteAll(); // Ŭ���� �� ����� �� ����
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToHome()
    {
        SceneManager.LoadScene("MainUI");
    }

    public void RestartFromBeginning()
    {
        // ���� �ʱ�ȭ: ���尪 ���� + �� �����
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void SavePlayerPosition(Transform player)
    {
        PlayerPrefs.SetFloat("PlayerX", player.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.position.y);
    }

    void LoadPlayerPosition(Transform player)
    {
        if (PlayerPrefs.HasKey("PlayerX") && PlayerPrefs.HasKey("PlayerY"))
        {
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");
            player.position = new Vector3(x, y, player.position.z);
        }
    }
}
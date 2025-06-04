using UnityEngine;

public class Carrot : MonoBehaviour
{
    public GameManager gameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameManager.TriggerClear();
            gameObject.SetActive(false); // ��� �����
        }
    }
}
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject carrotObject;

    private Animator animator;
    private bool isOpened = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (carrotObject != null)
        {
            carrotObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isOpened && collision.collider.CompareTag("Player"))
        {
            isOpened = true;
            animator.SetTrigger("Open");

            Invoke(nameof(RemoveChest), 1.5f); // 1.5�� �� ���� ����
            Invoke(nameof(ShowCarrot), 2f); // 2�� �� ��� ����
        }
    }

    void RemoveChest()
    {
        gameObject.SetActive(false);
    }

    void ShowCarrot()
    {
        if (carrotObject != null)
        {
            carrotObject.SetActive(true);
        }
    }
}
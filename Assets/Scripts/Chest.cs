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

            Invoke(nameof(RemoveChest), 1.5f); // 1.5초 후 상자 제거
            Invoke(nameof(ShowCarrot), 2f); // 2초 후 당근 등장
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
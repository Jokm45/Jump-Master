using UnityEngine;

public class BreakableTrigger : MonoBehaviour
{
    private BreakableGroup group;

    void Start()
    {
        group = GetComponentInParent<BreakableGroup>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            group.TriggerBreak(); // 부모에 알림
        }
    }
}
using UnityEngine;

public class SideOnlyZone : MonoBehaviour
{
    private Collider2D zoneCollider;

    private void Start()
    {
        zoneCollider = GetComponent<Collider2D>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log(" TriggerStay �۵���");

        var player = other.GetComponent<PlayerMovement>();
        var rb = other.GetComponent<Rigidbody2D>();
        var col = other.GetComponent<Collider2D>();

        if (player != null && rb != null && col != null)
        {
            Debug.Log(" Player ������, collider: " + col.bounds);

            if (zoneCollider.bounds.Intersects(col.bounds))
            {
                Debug.Log(" Zone �浹 ���� ����");
                player.EnterSideOnlyZone();
                rb.linearVelocity = Vector2.zero;
                rb.gravityScale = 0f;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var player = other.GetComponent<PlayerMovement>();
        var rb = other.GetComponent<Rigidbody2D>();
        if (player != null && rb != null)
        {
            player.ExitSideOnlyZone();
            rb.gravityScale = 1f;
        }
    }

    private bool IsFullyInside(Bounds inner, Bounds outer)
    {
        return outer.Contains(inner.min) && outer.Contains(inner.max);
    }
}

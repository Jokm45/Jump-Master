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

        var player = other.GetComponent<PlayerMovement>();
        var rb = other.GetComponent<Rigidbody2D>();
        var col = other.GetComponent<Collider2D>();

        if (player != null && rb != null && col != null)
        {
            if (zoneCollider.bounds.Intersects(col.bounds))
            {
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
}

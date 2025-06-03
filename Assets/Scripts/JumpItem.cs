using UnityEngine;

public class JumpItem : MonoBehaviour
{
    public float respawnDelay = 3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.SetJumpReady();
            }

            gameObject.SetActive(false);
            Invoke(nameof(Respawn), respawnDelay);
        }
    }

    void Respawn()
    {
        gameObject.SetActive(true);
    }
}
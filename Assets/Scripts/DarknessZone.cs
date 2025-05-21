using UnityEngine;

public class DarknessZone : MonoBehaviour
{
    public GameObject darkOverlay;
    public GameObject playerMask;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (darkOverlay != null) darkOverlay.SetActive(true);
            if (playerMask != null) playerMask.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (darkOverlay != null) darkOverlay.SetActive(false);
            if (playerMask != null) playerMask.SetActive(false);
        }
    }
}

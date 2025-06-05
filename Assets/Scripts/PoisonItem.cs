using UnityEngine;

public class PoisonItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement move = other.GetComponent<PlayerMovement>();
            if (move != null)
            {
                move.moveSpeed = 1f;
            }

            FindAnyObjectByType<ItemSpawner>()?.SpawnAntidote();
            Destroy(gameObject);
        }
    }
}
using UnityEngine;

public class AntidoteItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement move = other.GetComponent<PlayerMovement>();
            if (move != null)
            {
                move.moveSpeed = 3f;
            }

            FindAnyObjectByType<ItemSpawner>()?.SpawnPoison();
            Destroy(gameObject);
        }
    }
}
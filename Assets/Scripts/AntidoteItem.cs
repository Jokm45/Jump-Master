using UnityEngine;

public class AntidoteItem : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            PlayerMovement move = collision.gameObject.GetComponent<PlayerMovement>();
            if (move != null)
            {
                move.moveSpeed = 3f;
            }

            FindAnyObjectByType<ItemSpawner>()?.SpawnPoison();
            Destroy(gameObject);
        }
    }

}

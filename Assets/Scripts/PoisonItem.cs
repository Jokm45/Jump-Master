using UnityEngine;

public class PoisonItem : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"[Poison] �浹�� ������Ʈ: {collision.gameObject.name}");
        if (collision.gameObject.CompareTag("Player"))
        {
            
            Debug.Log("[Poison] Player�� ����!");
            PlayerMovement move = collision.gameObject.GetComponent<PlayerMovement>();
            if (move != null)
            {
                move.SetMoveSpeed(move.poisonSpeed);
            }

            FindAnyObjectByType<ItemSpawner>()?.SpawnAntidote();
            Destroy(gameObject);
        }
    }

}

using UnityEngine;

public class Stalactite : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

        if (collision.collider.CompareTag("Player"))
        {
            PlayerStun ps = collision.collider.GetComponent<PlayerStun>();
            if (ps != null)
            {
                ps.Stun();
            }
            Destroy(gameObject);
        }
    }
}


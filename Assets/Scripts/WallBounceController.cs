using UnityEngine;

public class WallBounceController : MonoBehaviour
{
    public float upwardBounceFactor = 1.3f;   // 상승 중 반발 계수
    public float downwardBounceFactor = 0.5f; // 하강 중 반발 계수
    public float maxHorizontalBounce = 8f;    // 튕기는 최대 수평 속도
    public float verticalFrictionFactor = 0.8f; // Y속도 감쇠 (마찰 느낌)

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            ContactPoint2D contact = collision.GetContact(0);
            Vector2 normal = contact.normal;
            Vector2 incomingVelocity = rb.linearVelocity;

            Vector2 reflected = Vector2.Reflect(incomingVelocity, normal).normalized;
            float strength = incomingVelocity.magnitude * 0.8f;

            // x 방향 보정
            if (Mathf.Abs(reflected.x) < 0.1f)
                reflected.x = (normal.x > 0) ? -1f : 1f;

            // y 방향 보정 (중요!)
            if (reflected.y <= 0.1f)
                reflected.y = 0.6f;

            rb.linearVelocity = reflected * Mathf.Max(strength, 4f);

            Debug.Log("최종 반동 속도: " + rb.linearVelocity);
        }
    }
}

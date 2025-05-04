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
            Debug.Log("벽에 부딪힘! velocity: " + rb.linearVelocity);
            ContactPoint2D contact = collision.GetContact(0);
            Debug.Log("벽에 충돌! normal: " + contact.normal + ", velocity: " + rb.linearVelocity);
            Vector2 normal = contact.normal.normalized;
            Vector2 currentVel = rb.linearVelocity;

            float bounceFactor = (currentVel.y > 0f) ? upwardBounceFactor : downwardBounceFactor;

            // 벽에 박은 각도 기준 수평 반발 계산 (속도 * 반발 계수 * 방향)
            float bounceX = -Vector2.Dot(currentVel, normal) * normal.x * bounceFactor;
            bounceX = Mathf.Clamp(bounceX, -maxHorizontalBounce, maxHorizontalBounce);

            // 수직 속도는 살짝 감쇠 (벽 마찰 효과)
            float newY = currentVel.y * verticalFrictionFactor;

            rb.linearVelocity = new Vector2(bounceX, newY);
        }
    }
}

using UnityEngine;

public class WallBounceController : MonoBehaviour
{
    public float upwardBounceFactor = 1.3f;   // ��� �� �ݹ� ���
    public float downwardBounceFactor = 0.5f; // �ϰ� �� �ݹ� ���
    public float maxHorizontalBounce = 8f;    // ƨ��� �ִ� ���� �ӵ�
    public float verticalFrictionFactor = 0.8f; // Y�ӵ� ���� (���� ����)

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            Debug.Log("���� �ε���! velocity: " + rb.linearVelocity);
            ContactPoint2D contact = collision.GetContact(0);
            Debug.Log("���� �浹! normal: " + contact.normal + ", velocity: " + rb.linearVelocity);
            Vector2 normal = contact.normal.normalized;
            Vector2 currentVel = rb.linearVelocity;

            float bounceFactor = (currentVel.y > 0f) ? upwardBounceFactor : downwardBounceFactor;

            // ���� ���� ���� ���� ���� �ݹ� ��� (�ӵ� * �ݹ� ��� * ����)
            float bounceX = -Vector2.Dot(currentVel, normal) * normal.x * bounceFactor;
            bounceX = Mathf.Clamp(bounceX, -maxHorizontalBounce, maxHorizontalBounce);

            // ���� �ӵ��� ��¦ ���� (�� ���� ȿ��)
            float newY = currentVel.y * verticalFrictionFactor;

            rb.linearVelocity = new Vector2(bounceX, newY);
        }
    }
}

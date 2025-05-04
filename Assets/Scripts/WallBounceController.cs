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
            ContactPoint2D contact = collision.GetContact(0);
            Vector2 normal = contact.normal;
            Vector2 incomingVelocity = rb.linearVelocity;

            Vector2 reflected = Vector2.Reflect(incomingVelocity, normal).normalized;
            float strength = incomingVelocity.magnitude * 0.8f;

            // x ���� ����
            if (Mathf.Abs(reflected.x) < 0.1f)
                reflected.x = (normal.x > 0) ? -1f : 1f;

            // y ���� ���� (�߿�!)
            if (reflected.y <= 0.1f)
                reflected.y = 0.6f;

            rb.linearVelocity = reflected * Mathf.Max(strength, 4f);

            Debug.Log("���� �ݵ� �ӵ�: " + rb.linearVelocity);
        }
    }
}

using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float combo;
    public float multiplier = 1;

    Rigidbody2D rb;
    Vector3 startPos;

    float jumpForce;
    float timeInComboRange;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        multiplier = Mathf.Clamp(1 + combo / 10, 1, 2);

        PogoJump();
        RotatePlayer();
        Respawn();
    }

    private void PogoJump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            jumpForce = Mathf.Clamp(jumpForce + Time.deltaTime * 15, 0, 10);
        }
        else if (jumpForce > 0)
        {
            rb.AddForce(Vector2.up * jumpForce * 100f * multiplier);
            StartCoroutine(ResetJump());

            jumpForce = 0;

            if (IsGrounded())
            {
                combo += 1;
            }
            else
            {
                combo = 0;
            }
        }

        if (IsGrounded())
        {
            timeInComboRange += Time.deltaTime;
        }
        else
        {
            timeInComboRange = 0;
        }

        if (IsGrounded() && timeInComboRange > 0.25f)
        {
            combo = 0;
        }
    }

    private void RotatePlayer()
    {
        Vector2 direction = rb.linearVelocity.normalized;
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            rb.MoveRotation(Mathf.LerpAngle(rb.rotation, angle, Time.deltaTime * 10f));
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));
    }

    private void Respawn()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = startPos;
            transform.rotation = Quaternion.identity;
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            startPos = transform.position;
        }
    }

    private IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(0.2f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsGrounded() && collision.gameObject.CompareTag("Wall"))
        {
            ContactPoint2D contact = collision.contacts[0];
            Vector2 normal = contact.normal;

            if (Mathf.Abs(normal.x) > 0.5f)
            {
                float bounceForce = 600f;
                rb.AddForce(new Vector2(normal.x * bounceForce, 0f));
            }
        }
    }
}

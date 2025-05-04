using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //public float moveSpeed = 2f;
    //public float jumpForce = 5f;

    public float maxChargeTime;
    public float maxJumpForce;
    public float horizontalForceMultiplier;
    public float verticalForceMultiplier;

    private Rigidbody2D rb;
    private float chargeTime = 0f;
    private bool isCharging = false;
    private Vector2 inputDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float height = Input.GetAxis("Horizontal");
        inputDirection = new Vector2(height, 1).normalized;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCharging = true;
            chargeTime = 0f;
        }

        if (isCharging)
        {
            chargeTime = chargeTime + Time.deltaTime;
            chargeTime = Mathf.Min(chargeTime, maxChargeTime);
        }

        if (Input.GetKeyUp(KeyCode.Space) && isCharging)
        {
            float powerRatio = chargeTime / maxChargeTime;

            // x ÈûÀº ÀÛ°Ô, y ÈûÀº ¼¼°Ô
            Vector2 jumpForce = new Vector2(
                inputDirection.x * horizontalForceMultiplier,
                verticalForceMultiplier
            ) * maxJumpForce * powerRatio;

            //rb.linearVelocity = Vector2.zero;
            rb.AddForce(jumpForce, ForceMode2D.Impulse);

            isCharging = false;
        }

        //// ÁÂ¿ì ÀÌµ¿
        //float moveInput = Input.GetAxisRaw("Horizontal");
        //rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        //// Á¡ÇÁ
        //if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        //{
        //    isJump = true;
        //    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //}
    }

    // ¶¥¿¡ ´ê¾Ò´ÂÁö È®ÀÎ
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isCharging = false;
        }
    }
}

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2f;                      // 좌우 이동 속도
    public float maxChargeTime = 1.5f;                // 점프 충전 최대 시간
    public float maxJumpForce = 25f;                  // 최대 점프 힘
    public float horizontalForceMultiplier = 1f;      // 좌우 점프 힘 비율
    public float verticalForceMultiplier = 1.5f;      // 위쪽 점프 힘 비율

    private Rigidbody2D rb;
    private float chargeTime = 0f;
    private bool isCharging = false;
    private bool isGrounded = false;
    private bool isJumping = false;                   // 점프 상태
    private bool hasBounced = false;                  // 벽에 튕긴 적 있는지
    private Vector2 inputDirection = Vector2.up;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 점프 충전 시작
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isCharging = true;
            chargeTime = 0f;
            inputDirection = Vector2.up;
        }

        // 점프 충전 중: 방향키로 방향 설정
        if (isCharging)
        {
            chargeTime += Time.deltaTime;
            chargeTime = Mathf.Min(chargeTime, maxChargeTime);

            float h = Input.GetAxisRaw("Horizontal");
            inputDirection = new Vector2(h, 1f).normalized;
        }

        // 점프 실행
        if (Input.GetKeyUp(KeyCode.Space) && isCharging)
        {
            float powerRatio = chargeTime / maxChargeTime;

            Vector2 jumpForce = new Vector2(
                inputDirection.x * horizontalForceMultiplier,
                inputDirection.y * verticalForceMultiplier
            ) * maxJumpForce * powerRatio;

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(jumpForce, ForceMode2D.Impulse);

            isCharging = false;
            isGrounded = false;
            isJumping = true;
            hasBounced = false;
        }

        // 좌우 이동 (점프 충전 중이 아니고, 땅에 있을 때만)
        if (!isCharging && isGrounded)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
    }

    // 충돌 처리
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 바닥에 닿았을 때
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isCharging = false;
            isJumping = false;
            hasBounced = false;
        }

        // 벽에 닿았을 때 (점프 중이고 아직 튕긴 적 없다면)
        if (collision.gameObject.CompareTag("Wall") && isJumping && !hasBounced)
        {
            Vector2 bounceDir;

            // 왼쪽 벽에 부딪힘 → 오른쪽 위로 튕김
            if (transform.position.x < collision.transform.position.x)
                bounceDir = new Vector2(1f, 1f).normalized;
            else // 오른쪽 벽에 부딪힘 → 왼쪽 위로 튕김
                bounceDir = new Vector2(-1f, 1f).normalized;

            float bounceForce = 5f; // 전체 튕김 세기 (X+Y 합쳐서 한 번에 줌)

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(bounceDir * bounceForce, ForceMode2D.Impulse);

            hasBounced = true;
            Debug.Log("튕김 방향: " + bounceDir * bounceForce);
        }
    }
}

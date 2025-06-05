using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f; // 좌우 이동 속도
    public float maxChargeTime = 1f; // 점프 충전 최대 시간
    public float maxJumpForce = 8f; // 최대 점프 힘
    public float horizontalForceMultiplier = 1f; // 좌우 점프 힘 비율
    public float verticalForceMultiplier = 2f; // 위쪽 점프 힘 비율
    public float bounceForceMultiplier = 1.0f; // 튕김 세기 계수 (기본값: 1.0)

    public AudioClip jumpSound;
    public AudioClip stunnedSound;

    private AudioSource playerAudio;
    private PlayerStun playerStun;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float chargeTime = 0f;
    private bool isCharging = false;
    private bool isGrounded = false;
    private bool isJumping = false;
    private bool canJumpFromItem = false;
    private bool inSideOnlyZone = false;
    private bool wasStunned = false;

    private Vector2 inputDirection = Vector2.up;
    private Vector2 jumpStartPosition;
    private Vector2 lastJumpForce;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerStun = GetComponent<PlayerStun>();
        playerAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (inSideOnlyZone)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, 0f); // Y속도 완전 고정
            return; // 다른 입력 차단 (점프 등)
        }

        // 기절 중이면 점프 충전 중지
        if (playerStun != null && playerStun.IsStunned())
        {
            if (!wasStunned)
            {
                playerAudio.PlayOneShot(stunnedSound);
                wasStunned = true;
            }
            isCharging = false;
            return;
        }
        else
        {
            wasStunned = false;
        }

        // 점프 충전 시작
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || canJumpFromItem) && !isJumping)
        {
            isCharging = true;
            chargeTime = 0f;
            inputDirection = Vector2.up;
            rb.linearVelocity = Vector2.zero;
        }

        // 충전 중 방향 설정
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
            float minRatio = 0.4f; // 최소 점프 비율 (40%)
            float powerRatio = Mathf.Max(minRatio, chargeTime / maxChargeTime);

            Vector2 jumpForce = new Vector2(
                inputDirection.x * horizontalForceMultiplier,
                inputDirection.y * verticalForceMultiplier
            ) * maxJumpForce * powerRatio;

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(jumpForce, ForceMode2D.Impulse);

            playerAudio.PlayOneShot(jumpSound);

            jumpStartPosition = transform.position;
            lastJumpForce = jumpForce;

            isCharging = false;
            isGrounded = false;
            isJumping = true;
            canJumpFromItem = false;
        }

        // 이동 (점프 중이 아니고, 땅에 있을 때만)
        if (!isCharging && isGrounded)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }

        // 방향 입력에 따라 스프라이트 반전 (이동, 충전할 때)
        float inputX = Input.GetAxisRaw("Horizontal");
        if (inputX != 0)
            spriteRenderer.flipX = inputX < 0;

        // 애니메이션 파라미터 업데이트
        animator.SetBool("isCharging", isCharging);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isFalling", rb.linearVelocity.y < -0.1f && !isGrounded);
        animator.SetBool("isMoving", Mathf.Abs(rb.linearVelocity.x) > 0.1f && isGrounded);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 바닥 감지
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 충돌 방향(normal)이 위쪽을 향할 때만 지면으로 인정
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    isGrounded = true;
                    isCharging = false;
                    isJumping = false;
                    canJumpFromItem = false;
                    animator.Play("Idle"); // 착지 시 Idle 상태로 전환
                    break;
                }
            }
        }

        // 벽 튕김
        if (collision.gameObject.CompareTag("Wall") && isJumping)
        {
            Vector2 velocity = rb.linearVelocity.magnitude < 0.1f ? lastJumpForce : rb.linearVelocity;
            Vector2 normal = collision.contacts[0].normal;

            Vector2 bounceDir = Vector2.Reflect(velocity.normalized, normal); // 반사 방향 계산
            Vector2 contactPoint = collision.contacts[0].point;
            float dir = transform.position.x < contactPoint.x ? -1f : 1f; // 오른쪽 벽에 부딪히면 왼쪽(-1), 왼쪽 벽이면 오른쪽(+1)
            bounceDir.x = dir * 0.5f; // x축 보정
            bounceDir = bounceDir.normalized;

            // 튕김 세기 계산
            float movedDistance = Vector2.Distance(jumpStartPosition, transform.position);
            float intendedDistance = lastJumpForce.magnitude;
            float closeRatio = Mathf.Clamp01(1f - (movedDistance / intendedDistance));
            float bouncePower = Mathf.Max(1f, lastJumpForce.magnitude * closeRatio * bounceForceMultiplier);

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(bounceDir * bouncePower, ForceMode2D.Impulse);
        }
    }
    public void EnterSideOnlyZone()
    {
        inSideOnlyZone = true;
        isCharging = false;
        isJumping = false;
    }

    public void ExitSideOnlyZone()
    {
        inSideOnlyZone = false;
    }

    public void SetJumpReady()
    {
        canJumpFromItem = true;
        isJumping = false;
    }
}
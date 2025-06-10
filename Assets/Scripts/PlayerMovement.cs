using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f; // 좌우 이동 속도
    public float maxChargeTime = 1f; // 점프 충전 최대 시간
    public float maxJumpForce = 8f; // 최대 점프 힘
    public float horizontalForceMultiplier = 1f; // 점프 시 좌우 힘 계수
    public float verticalForceMultiplier = 2f; // 점프 시 위쪽 힘 계수
    public float bounceForceMultiplier = 1.0f; // 벽 튕김 힘 계수

    public AudioClip jumpSound; // 점프 사운드
    public AudioClip stunnedSound; // 기절 사운드

    private AudioSource playerAudio;
    private PlayerStun playerStun;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float chargeTime = 0f; // 점프 충전 시간
    private bool isCharging = false; // 점프 충전 중 여부
    private bool isGrounded = false; // 지면에 닿았는지
    private bool isJumping = false; // 점프 중인지
    private bool canJumpFromItem = false; // 아이템 점프 가능 여부
    private bool inSideOnlyZone = false; // 공중이동가능 구역 여부
    private bool wasStunned = false; // 기절 상태여부

    private Vector2 inputDirection = Vector2.up; // 점프 방향 (위)
    private Vector2 jumpStartPosition; // 점프 시작 위치 (튕김 계산용)
    private Vector2 lastJumpForce; // 마지막 점프 힘 (튕김 계산용)

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
        // 공중이동가능 구역에서는 이동만 허용
        if (inSideOnlyZone)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, 0f); // Y속도 완전 고정
            return;
        }

        // 기절 상태면 점프 충전 중지
        if (playerStun != null && playerStun.IsStunned())
        {
            if (!wasStunned)
            {
                playerAudio.PlayOneShot(stunnedSound); // 기절 사운드 재생
                wasStunned = true;
            }
            isCharging = false;
            return;
        }
        else
        {
            wasStunned = false;
        }

        // 스페이스바 누르면 점프 충전 시작
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || canJumpFromItem) && !isJumping)
        {
            isCharging = true; // 충전 시작
            chargeTime = 0f; // 충전 시간 초기화
            inputDirection = Vector2.up; // 기본 방향은 위쪽
            rb.linearVelocity = Vector2.zero; // 충돌로 인한 잔여 속도 제거
        }

        // 점프 충전 중 방향 설정
        if (isCharging)
        {
            chargeTime += Time.deltaTime; // 충전 시간 누적
            chargeTime = Mathf.Min(chargeTime, maxChargeTime); // 최대 충전 시간 제한

            float h = Input.GetAxisRaw("Horizontal"); // 좌우 방향 입력
            inputDirection = new Vector2(h, 1f).normalized;
        }

        // 스페이스바 떼면 점프 실행
        if (Input.GetKeyUp(KeyCode.Space) && isCharging)
        {
            float minRatio = 0.4f; // 최소 점프 충전 비율
            float powerRatio = Mathf.Max(minRatio, chargeTime / maxChargeTime); // 점프 세기 비율

            // 최종 점프 벡터 계산
            Vector2 jumpForce = new Vector2(
                inputDirection.x * horizontalForceMultiplier,
                inputDirection.y * verticalForceMultiplier
            ) * maxJumpForce * powerRatio;

            rb.linearVelocity = Vector2.zero; // 점프 전에 기존 속도 초기화
            rb.AddForce(jumpForce, ForceMode2D.Impulse); // 점프 실행

            playerAudio.PlayOneShot(jumpSound); // 점프 사운드 재생

            jumpStartPosition = transform.position; // 튕김 계산용 위치 저장
            lastJumpForce = jumpForce; // 튕김 계산용 점프 벡터 저장

            isCharging = false;
            isGrounded = false;
            isJumping = true;
            canJumpFromItem = false;
        }

        // 땅 위에 있을 때 좌우 이동
        if (!isCharging && isGrounded)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }

        // 입력 방향에 따라 스프라이트 반전
        float inputX = Input.GetAxisRaw("Horizontal");
        if (inputX != 0)
            spriteRenderer.flipX = inputX < 0;

        // 애니메이션 파라미터 설정
        animator.SetBool("isCharging", isCharging);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isFalling", rb.linearVelocity.y < -0.1f && !isGrounded);
        animator.SetBool("isMoving", Mathf.Abs(rb.linearVelocity.x) > 0.1f && isGrounded);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 지면 착지 판정
        if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f) // 위쪽에서의 충돌만 인정
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

        // 벽에 충돌하면 튕김 처리
        if (collision.gameObject.CompareTag("Wall") && isJumping)
        {
            // 속도가 거의 0이면 벽 반사 방향 계산을 위해 마지막 점프 힘을 대신 사용
            Vector2 velocity = rb.linearVelocity.magnitude < 0.1f ? lastJumpForce : rb.linearVelocity;
            Vector2 normal = collision.contacts[0].normal; // 충돌한 면의 방향 벡터

            Vector2 bounceDir = Vector2.Reflect(velocity.normalized, normal); // 현재 속도의 단위 벡터를 기준으로 반사 방향 계산
            Vector2 contactPoint = collision.contacts[0].point; // 충돌한 좌표 위치
            float dir = transform.position.x < contactPoint.x ? -1f : 1f; // 오른쪽 벽에 부딪히면 왼쪽(-1), 왼쪽 벽이면 오른쪽(+1)
            bounceDir.x = dir * 0.5f; // x축 보정 - 너무 강한 튕김 방지 및 제어된 반사 방향 유도
            bounceDir = bounceDir.normalized; // 벡터 정규화

            // 이동 거리와 원래 점프 거리로 튕김 세기 결정
            float movedDistance = Vector2.Distance(jumpStartPosition, transform.position); // 실제 이동 거리
            float intendedDistance = lastJumpForce.magnitude; // 원래 점프 힘의 크기 (의도된 이동 거리)

            // 실제 이동이 짧을수록 튕김 세기를 강하게 유지하기 위한 비율 계산 (0~1)
            float closeRatio = Mathf.Clamp01(1f - (movedDistance / intendedDistance));
            // 튕김 세기 계산 - 가까울수록 강함 (최소 1)
            float bouncePower = Mathf.Max(1f, lastJumpForce.magnitude * closeRatio * bounceForceMultiplier);

            rb.linearVelocity = Vector2.zero; // 기존 속도를 0으로 초기화
            rb.AddForce(bounceDir * bouncePower, ForceMode2D.Impulse); // 계산된 반사 방향과 세기를 적용해 튕김 힘 작용
        }
    }

    // 공중이동가능 구역 진입 시 호출
    public void EnterSideOnlyZone()
    {
        inSideOnlyZone = true;
        isCharging = false;
        isJumping = false;
    }

    // 공중이동가능 구역에서 벗어남
    public void ExitSideOnlyZone()
    {
        inSideOnlyZone = false;
    }

    // 아이템을 통해 점프 가능 상태로 전환
    public void SetJumpReady()
    {
        canJumpFromItem = true;
        isJumping = false;
    }
}
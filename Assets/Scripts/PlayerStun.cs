using UnityEngine;

public class PlayerStun : MonoBehaviour
{
    public float stunDuration = 2f;
    public GameObject stunEffectPrefab;
    public Sprite stunnedSprite; // 기절 이미지

    private bool isStunned = false;
    private float stunTimer = 0f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private GameObject activeStunEffect;
    private Sprite originalSprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite; // 원래 스프라이트 저장
    }

    void Update()
    {
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                isStunned = false;

                // 이동 제약 복구
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;

                // 애니메이션 복구
                spriteRenderer.sprite = originalSprite;
                if (animator != null)
                {
                    animator.enabled = true;
                    animator.Play("Idle");
                }

                // 이펙트 제거
                if (activeStunEffect != null)
                    Destroy(activeStunEffect);
            }
        }
    }

    public void Stun()
    {
        isStunned = true;
        stunTimer = stunDuration;

        // 이동 정지 + 제약
        rb.linearVelocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        // 애니메이션 끄고 기절 스프라이트로 교체
        if (animator != null)
            animator.enabled = false;

        if (spriteRenderer != null && stunnedSprite != null)
            spriteRenderer.sprite = stunnedSprite;

        // 이펙트 생성
        if (stunEffectPrefab != null)
        {
            Vector3 offset = new Vector3(0f, 0f, 0f);
            activeStunEffect = Instantiate(stunEffectPrefab, transform.position + offset, Quaternion.identity, transform);
        }

        Debug.Log("스턴");
    }

    public bool IsStunned()
    {
        return isStunned;
    }
}
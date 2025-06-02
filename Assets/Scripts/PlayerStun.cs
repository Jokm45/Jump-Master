using UnityEngine;

public class PlayerStun : MonoBehaviour
{
    public float stunDuration = 2f;
    public GameObject stunEffectPrefab;

    private bool isStunned = false;
    private float stunTimer = 0f;

    private Rigidbody2D rb;
    private Animator animator;
    private GameObject activeStunEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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

             
                if (animator != null)
                {
                    animator.speed = 1f;
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

        // 애니메이션 멈춤
        if (animator != null)
            animator.speed = 0f;

        // 기절 이펙트 생성
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

using UnityEngine;

public class BreakableGroup : MonoBehaviour
{
    public GameObject[] blocks;
    private bool triggered = false; // 붕괴 여부
    private Vector3[] originalPositions;

    void Start()
    {
        originalPositions = new Vector3[blocks.Length];
        for (int i = 0; i < blocks.Length; i++)
        {
            originalPositions[i] = blocks[i].transform.position;
        }
    }

    public void TriggerBreak()
    {
        if (triggered) return;
        triggered = true;

        Invoke(nameof(DropBlocks), 2f);  // 2초 후 낙하
        Invoke(nameof(ResetBlocks), 6f);  // +4초 뒤 복구
    }

    void DropBlocks()
    {
        foreach (var block in blocks)
        {
            var rb = block.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic; // 낙하 시작
            }
        }
    }

    void ResetBlocks()
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            var rb = blocks[i].GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            blocks[i].transform.position = originalPositions[i]; // 위치 초기화
            blocks[i].transform.rotation = Quaternion.identity; // 회전 초기화
        }

        triggered = false;
    }
}
using UnityEngine;

public class BreakableGroup : MonoBehaviour
{
    public GameObject[] blocks;
    private bool triggered = false; // �ر� ����
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

        Invoke(nameof(DropBlocks), 2f);  // 2�� �� ����
        Invoke(nameof(ResetBlocks), 6f);  // +4�� �� ����
    }

    void DropBlocks()
    {
        foreach (var block in blocks)
        {
            var rb = block.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic; // ���� ����
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

            blocks[i].transform.position = originalPositions[i]; // ��ġ �ʱ�ȭ
            blocks[i].transform.rotation = Quaternion.identity; // ȸ�� �ʱ�ȭ
        }

        triggered = false;
    }
}
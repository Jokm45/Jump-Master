using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    private Vector2? highestCheckpointPos = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            Vector2 newCheckpointPos = other.GetComponent<Checkpoint>().position;

            // ó�� �����̰ų� �� ���� ��ġ�� ����
            if (highestCheckpointPos == null || newCheckpointPos.y > highestCheckpointPos.Value.y)
            {
                highestCheckpointPos = newCheckpointPos;
                Debug.Log("üũ����Ʈ �����: " + newCheckpointPos);
            }
        }
    }

    // ��: R Ű�� ����� ��ġ�� �����̵�
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && highestCheckpointPos != null)
        {
            transform.position = highestCheckpointPos.Value;
            Debug.Log("üũ����Ʈ�� �̵�!");
        }
    }
}

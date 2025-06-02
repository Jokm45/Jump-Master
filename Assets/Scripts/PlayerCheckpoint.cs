using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    private Vector2? highestCheckpointPos = null;
    private Checkpoint activeCheckpoint = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            Checkpoint checkpoint = other.GetComponent<Checkpoint>();
            Vector2 newCheckpointPos = checkpoint.position;

            if (highestCheckpointPos == null || newCheckpointPos.y > highestCheckpointPos.Value.y)
            {
                highestCheckpointPos = newCheckpointPos;

                // ���� üũ����Ʈ ���� ����
                if (activeCheckpoint != null)
                    activeCheckpoint.SetActiveSmoke(false);

                // �� üũ����Ʈ ���� �ѱ�
                checkpoint.SetActiveSmoke(true);
                activeCheckpoint = checkpoint;

                Debug.Log("üũ����Ʈ �����: " + newCheckpointPos);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && highestCheckpointPos != null)
        {
            transform.position = highestCheckpointPos.Value;
            Debug.Log("üũ����Ʈ�� �̵�!");
        }
    }
}
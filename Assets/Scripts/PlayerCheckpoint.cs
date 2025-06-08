using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    private Vector2? highestCheckpointPos = null;
    private Checkpoint activeCheckpoint = null;

    private void Start()
    {
        // ����� üũ����Ʈ ��ġ �ҷ�����
        if (PlayerPrefs.HasKey("CheckpointX") && PlayerPrefs.HasKey("CheckpointY"))
        {
            float x = PlayerPrefs.GetFloat("CheckpointX");
            float y = PlayerPrefs.GetFloat("CheckpointY");
            highestCheckpointPos = new Vector2(x, y);

            // Checkpoint �� ��ġ�� ��ġ�ϴ� ���� ã�� ���� �ѱ�
            GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
            foreach (GameObject obj in checkpoints)
            {
                Checkpoint checkpoint = obj.GetComponent<Checkpoint>();
                if (checkpoint != null && Vector2.Distance(checkpoint.position, highestCheckpointPos.Value) < 0.01f)
                {
                    checkpoint.SetActiveSmoke(true);
                    activeCheckpoint = checkpoint;
                    break;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            Checkpoint checkpoint = other.GetComponent<Checkpoint>();
            Vector2 newCheckpointPos = checkpoint.position;

            if (highestCheckpointPos == null || newCheckpointPos.y > highestCheckpointPos.Value.y)
            {
                highestCheckpointPos = newCheckpointPos;

                // ��ġ ����
                PlayerPrefs.SetFloat("CheckpointX", newCheckpointPos.x);
                PlayerPrefs.SetFloat("CheckpointY", newCheckpointPos.y);

                // ���� üũ����Ʈ ���� ����
                if (activeCheckpoint != null)
                    activeCheckpoint.SetActiveSmoke(false);

                // �� üũ����Ʈ ���� �ѱ�
                checkpoint.SetActiveSmoke(true);
                activeCheckpoint = checkpoint;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && highestCheckpointPos != null)
        {
            transform.position = highestCheckpointPos.Value;
        }
    }
}
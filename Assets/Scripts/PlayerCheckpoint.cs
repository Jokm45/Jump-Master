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

                // 이전 체크포인트 연기 끄기
                if (activeCheckpoint != null)
                    activeCheckpoint.SetActiveSmoke(false);

                // 새 체크포인트 연기 켜기
                checkpoint.SetActiveSmoke(true);
                activeCheckpoint = checkpoint;

                Debug.Log("체크포인트 저장됨: " + newCheckpointPos);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && highestCheckpointPos != null)
        {
            transform.position = highestCheckpointPos.Value;
            Debug.Log("체크포인트로 이동!");
        }
    }
}
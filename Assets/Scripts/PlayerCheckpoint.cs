using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    private Vector2? highestCheckpointPos = null;
    private Checkpoint activeCheckpoint = null;

    private void Start()
    {
        // 저장된 체크포인트 위치 불러오기
        if (PlayerPrefs.HasKey("CheckpointX") && PlayerPrefs.HasKey("CheckpointY"))
        {
            float x = PlayerPrefs.GetFloat("CheckpointX");
            float y = PlayerPrefs.GetFloat("CheckpointY");
            highestCheckpointPos = new Vector2(x, y);

            // Checkpoint 중 위치가 일치하는 것을 찾아 연기 켜기
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

                // 위치 저장
                PlayerPrefs.SetFloat("CheckpointX", newCheckpointPos.x);
                PlayerPrefs.SetFloat("CheckpointY", newCheckpointPos.y);

                // 이전 체크포인트 연기 끄기
                if (activeCheckpoint != null)
                    activeCheckpoint.SetActiveSmoke(false);

                // 새 체크포인트 연기 켜기
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
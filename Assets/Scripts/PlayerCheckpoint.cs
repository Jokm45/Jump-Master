using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    private Vector2? highestCheckpointPos = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            Vector2 newCheckpointPos = other.GetComponent<Checkpoint>().position;

            // 처음 저장이거나 더 높은 위치면 저장
            if (highestCheckpointPos == null || newCheckpointPos.y > highestCheckpointPos.Value.y)
            {
                highestCheckpointPos = newCheckpointPos;
                Debug.Log("체크포인트 저장됨: " + newCheckpointPos);
            }
        }
    }

    // 예: R 키로 저장된 위치로 순간이동
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && highestCheckpointPos != null)
        {
            transform.position = highestCheckpointPos.Value;
            Debug.Log("체크포인트로 이동!");
        }
    }
}

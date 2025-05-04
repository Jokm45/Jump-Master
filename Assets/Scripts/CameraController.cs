using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float verticalSize = 12f;  // 카메라 한 화면 높이

    private int currentZone = 0;

    void Start()
    {
        currentZone = Mathf.RoundToInt(player.position.y / verticalSize);
        UpdateCameraPosition();
    }

    void LateUpdate()
    {
        float camCenterY = currentZone * verticalSize;
        float halfHeight = verticalSize / 2f;

        // 플레이어가 위로 조금이라도 벗어났으면 zone++
        if (player.position.y > camCenterY + halfHeight)
        {
            currentZone++;
        }
        // 플레이어가 아래로 조금이라도 벗어났으면 zone--
        else if (player.position.y < camCenterY - halfHeight)
        {
            currentZone--;
        }

        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        float camY = currentZone * verticalSize;
        transform.position = new Vector3(transform.position.x, camY, transform.position.z);
    }
}

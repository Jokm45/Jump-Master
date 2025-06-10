using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float verticalSize = 12f; // 카메라 한 화면의 높이 (한 구역 높이)

    private int currentZone = 0; // 현재 카메라가 위치한 구역 번호

    void Start()
    {
        // 시작 시 플레이어 위치 기준으로 현재 구역 번호 계산
        currentZone = Mathf.RoundToInt(player.position.y / verticalSize); // 반올림
        UpdateCameraPosition(); // 초기 카메라 위치 설정
    }

    void LateUpdate()
    {
        // 현재 카메라 중심의 y 좌표
        float camCenterY = currentZone * verticalSize;
        float halfHeight = verticalSize / 2f;

        // 플레이어가 화면 위쪽 경계를 넘으면 구역 상승
        if (player.position.y > camCenterY + halfHeight)
        {
            currentZone++;
        }
        // 플레이어가 화면 아래쪽 경계를 넘으면 구역 하강
        else if (player.position.y < camCenterY - halfHeight)
        {
            currentZone--;
        }

        // 구역이 바뀌었을 경우 카메라 위치 갱신
        UpdateCameraPosition();
    }

    // 카메라의 Y 위치를 현재 구역에 맞게 업데이트
    void UpdateCameraPosition()
    {
        float camY = currentZone * verticalSize;
        transform.position = new Vector3(transform.position.x, camY, transform.position.z);
    }
}
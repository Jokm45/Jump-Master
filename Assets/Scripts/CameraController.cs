using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float verticalSize = 12f;  // ī�޶� �� ȭ�� ����

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

        // �÷��̾ ���� �����̶� ������� zone++
        if (player.position.y > camCenterY + halfHeight)
        {
            currentZone++;
        }
        // �÷��̾ �Ʒ��� �����̶� ������� zone--
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

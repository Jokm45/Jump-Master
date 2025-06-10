using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float verticalSize = 12f; // ī�޶� �� ȭ���� ���� (�� ���� ����)

    private int currentZone = 0; // ���� ī�޶� ��ġ�� ���� ��ȣ

    void Start()
    {
        // ���� �� �÷��̾� ��ġ �������� ���� ���� ��ȣ ���
        currentZone = Mathf.RoundToInt(player.position.y / verticalSize); // �ݿø�
        UpdateCameraPosition(); // �ʱ� ī�޶� ��ġ ����
    }

    void LateUpdate()
    {
        // ���� ī�޶� �߽��� y ��ǥ
        float camCenterY = currentZone * verticalSize;
        float halfHeight = verticalSize / 2f;

        // �÷��̾ ȭ�� ���� ��踦 ������ ���� ���
        if (player.position.y > camCenterY + halfHeight)
        {
            currentZone++;
        }
        // �÷��̾ ȭ�� �Ʒ��� ��踦 ������ ���� �ϰ�
        else if (player.position.y < camCenterY - halfHeight)
        {
            currentZone--;
        }

        // ������ �ٲ���� ��� ī�޶� ��ġ ����
        UpdateCameraPosition();
    }

    // ī�޶��� Y ��ġ�� ���� ������ �°� ������Ʈ
    void UpdateCameraPosition()
    {
        float camY = currentZone * verticalSize;
        transform.position = new Vector3(transform.position.x, camY, transform.position.z);
    }
}
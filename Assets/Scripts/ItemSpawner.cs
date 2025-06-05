using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject poisonPrefab;
    public GameObject antidotePrefab;

    public Vector3 poisonSpawnPosition;
    public Vector3 antidoteSpawnPosition;

    public void SpawnPoison()
    {
        Instantiate(poisonPrefab, poisonSpawnPosition, Quaternion.identity);
    }

    public void SpawnAntidote()
    {
        Instantiate(antidotePrefab, antidoteSpawnPosition, Quaternion.identity);
    }

    private void Start()
    {
        // ���� ���� �� ������� ����
        SpawnPoison();
    }
}

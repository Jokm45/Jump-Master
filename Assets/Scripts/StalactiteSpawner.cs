using UnityEngine;

public class StalactiteSpawner : MonoBehaviour
{
    public  GameObject stalactitePrefab;
    public float Spawnrate = 5f;
    public float Timer = 0f;

    void Update()
    {
        Timer += Time.deltaTime;
        if(Timer >= Spawnrate)
        {
            Instantiate(stalactitePrefab, transform.position, Quaternion.identity);
            Timer = 0f;
        }
    }
}

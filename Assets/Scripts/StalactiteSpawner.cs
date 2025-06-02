using UnityEngine;

public class StalactiteSpawner : MonoBehaviour
{
    public  GameObject stalactitePrefab;
    public float Spawnrate = 5f;
    public float Timer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
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

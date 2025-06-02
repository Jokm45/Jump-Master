using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Vector2 position => transform.position;

    public GameObject smokeEffect; // smoke 오브젝트

    public void SetActiveSmoke(bool active)
    {
        if (smokeEffect == null) return;

        smokeEffect.SetActive(active);
    }
}
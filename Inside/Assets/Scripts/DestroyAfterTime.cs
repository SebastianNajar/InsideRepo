using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float timer;

    private void Start()
    {
        Destroy(gameObject, timer);
    }
}

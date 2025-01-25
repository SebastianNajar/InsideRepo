using UnityEngine;

public class ActivateEnemies : MonoBehaviour
{
    public GameObject[] enemies;

    public void SetEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(true);
        }
    }
}

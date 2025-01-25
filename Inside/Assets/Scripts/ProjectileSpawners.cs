using UnityEngine;

public class ProjectileSpawners : MonoBehaviour
{
    public GameObject[] spawners;
    public GameObject projectile;
    private float timer;
    
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > Random.Range(2, 5))
        {
            timer = 0;
            foreach(GameObject spawner in spawners)
            {
                int randomize = Random.Range(0, 2);
                if(randomize > 0)
                {
                    Instantiate(projectile, spawner.transform);
                }
            }
        }
    }
}

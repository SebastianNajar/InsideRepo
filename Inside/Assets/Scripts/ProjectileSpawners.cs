using UnityEngine;

public class ProjectileSpawners : MonoBehaviour
{
    public GameObject[] spawners;
    public GameObject projectile;
    private bool active = false;
    private float timer;
    
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > Random.Range(3, 6))
        {
            timer = 0;
            foreach(GameObject spawner in spawners)
            {
                int randomize = Random.Range(0, 5);
                if(randomize > 1)
                {
                    GameObject newProjectile = Instantiate(projectile, spawner.transform.position, spawner.transform.rotation);
                }
            }
        }
    }

    public void DestroySpawners()
    {
        Destroy(gameObject);
    }

    public void ActivateSpawners()
    {
        foreach (GameObject spawner in spawners)
        {
            spawner.SetActive(true);
            int randomize = Random.Range(0, 5);
            if (randomize > 1)
            {
                GameObject newProjectile = Instantiate(projectile, new Vector3(spawner.transform.position.x, spawner.transform.position.y - 20), spawner.transform.rotation);
            }
        }
    }
}

using Unity.Cinemachine;
using UnityEngine;

public class ParryProjectile: MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    private float timer;
    public float force;
    private bool isFriendly = false;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * force;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(timer > 10)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isFriendly && collision.gameObject.CompareTag("Player") && collision.name == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
            Destroy(gameObject);
        }
        else if (isFriendly && collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Projectile hit an enemy!");
            Destroy(collision.gameObject); 
            Destroy(gameObject); 
        }

    }

    public void ChangeTargetToEnemies()
    {
        isFriendly = true;
        //change color of projectile? optional
        GetComponent<SpriteRenderer>().color = Color.blue;
    }

}

using Unity.Cinemachine;
using UnityEngine;

public class ParryProjectile: MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    private float timer;
    private float parryTimer;
    public float force;
    private bool isFriendly = false;
    public BossBehavior bossBehavior;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        bossBehavior = GameObject.Find("Boss").GetComponent<BossBehavior>();

        Vector3 direction = player.transform.position - transform.position;
        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * force;
    }

    void Update()
    {
        timer += Time.deltaTime;
        parryTimer += Time.deltaTime;

        if(parryTimer > 6)
        {
            if (bossBehavior.fightStarted)
            {
                bossBehavior.OnParryFail();
            }
            parryTimer = 0;
        }

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
            if (bossBehavior.fightStarted)
            {
                Debug.Log("Pop up once");
                bossBehavior.OnParryFail();
            }
            Destroy(gameObject);
        }
        else if (isFriendly && collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Projectile hit an enemy!");
            collision.gameObject.GetComponent<EnemyShooting>().TakeDamage();
            Destroy(gameObject);
        }
        else if(isFriendly && collision.gameObject.CompareTag("Boss"))
        {
            collision.gameObject.GetComponent<BossBehavior>().OnParrySuccess();
            Destroy(gameObject);
        }

    }

    public void ChangeTargetToEnemies()
    {
        isFriendly = true;
    }

}

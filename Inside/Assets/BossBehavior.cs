using System.Collections;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    // General
    public GameObject parryableProjectilePrefab;
    public Transform projectileSpawnPoint; // Where the parryable projectile spawns
    private int currentPhase = 1;
    private float phaseTimer = 0f;
    private bool isPhaseActive = true;
    public bool fightStarted = false; // Tracks if the fight has started
    private Animator animator;
    public GameObject explosion;
    private float playerCollisionTimer = 0f;
    public PopupManager popup;

    // Phase 1
    public float phase1Duration;
    public float bounceSpeed = 5f;
    private Rigidbody2D rb;

    // Phase 2
    public GameObject binarySpawner;
    private ProjectileSpawners spawner;
    private bool alreadySpanwed = false;

    private Transform player;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spawner = binarySpawner.GetComponent<ProjectileSpawners>();
        player = GameObject.FindWithTag("Player").transform; // Find the player object
    }

    void Update()
    {
        playerCollisionTimer += Time.deltaTime;

        if (isPhaseActive && fightStarted)
        {
            switch (currentPhase)
            {
                case 1:
                    Phase1Behavior();
                    break;
                case 2:
                    Phase2Behavior();
                    break;
            }
        }
    }

    public void StartFight()
    {
        fightStarted = true;
        StartPhase1();
    }

    // Phase 1 Logic
    private void StartPhase1()
    {
        animator.SetTrigger("walk");
        currentPhase = 1;
        isPhaseActive = true;
        phaseTimer = 0f;
        rb.linearVelocity = GetDirectionToPlayer() * bounceSpeed;
    }

    private void Phase1Behavior()
    {
        // Make boss follow the player during this phase
        Vector2 directionToPlayer = GetDirectionToPlayer();
        rb.linearVelocity = directionToPlayer * bounceSpeed;

        // Update phase timer
        phaseTimer += Time.deltaTime;

        // Launch parryable projectile after phase duration
        if (phaseTimer >= phase1Duration)
        {
            LaunchParryableProjectile();
            isPhaseActive = false; // Temporarily stop behavior until parry resolves
        }
    }

    // Phase 2 Logic
    private IEnumerator StartPhase2()
    {
        yield return new WaitForSeconds(1);
        animator.SetTrigger("shoot");
        yield return new WaitForSeconds(1);

        animator.SetTrigger("walk");
        if (!alreadySpanwed)
        {
            Transform spawnerPos = binarySpawner.transform;
            spawnerPos.position = new Vector3(spawnerPos.position.x, spawnerPos.position.y + 13, spawnerPos.position.z);
            spawner.ActivateSpawners(true);
            alreadySpanwed = true;
            spawner.isBoss = true;
        }

        currentPhase = 2;
        isPhaseActive = true;
        phaseTimer = 0f;
        rb.linearVelocity = GetDirectionToPlayer() * bounceSpeed;
    }

    private void Phase2Behavior()
    {
        // Make boss follow the player during this phase
        Vector2 directionToPlayer = GetDirectionToPlayer();
        rb.linearVelocity = directionToPlayer * bounceSpeed;

        phaseTimer += Time.deltaTime;

        // Launch parryable projectile after phase duration
        if (phaseTimer >= phase1Duration)
        {
            LaunchParryableProjectile();
            isPhaseActive = false; // Temporarily stop behavior until parry resolves
        }
    }

    // Parry Handling
    private void LaunchParryableProjectile()
    {
        Debug.Log("Launching parryable projectile!");
        rb.linearVelocity = Vector2.zero;
        animator.SetTrigger("shoot");
        Instantiate(parryableProjectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
    }

    public void OnParrySuccess()
    {
        Debug.Log("Parry succeeded!");
        animator.SetTrigger("damage");

        switch (currentPhase)
        {
            case 1:
                StartCoroutine(StartPhase2());
                break;
            case 2:
                StartCoroutine(Die());
                break;
        }
    }

    public void OnParryFail()
    {
        Debug.Log("Parry failed!");
        switch (currentPhase)
        {
            case 1:
                StartPhase1();
                break;
            case 2:
                StartCoroutine(StartPhase2());
                break;
        }
    }

    // Boss Death
    private IEnumerator Die()
    {
        spawner.DisableSpawners();
        animator.SetTrigger("dead");

        yield return new WaitForSeconds(3);

        GameObject obj = Instantiate(explosion, this.transform.position, this.transform.rotation);
        Debug.Log("Boss defeated!");
        popup.ShowPopup();
        Destroy(gameObject);


    } 

    // Utility Methods 
    private Vector2 GetDirectionToPlayer()
    {
        // Get direction to player by subtracting the boss position from the player's position
        return (player.position - transform.position).normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player" && playerCollisionTimer > 2)
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
            playerCollisionTimer = 0;
        }
        // Reverse direction upon hitting walls
        Vector2 normal = collision.contacts[0].normal;
        rb.linearVelocity = Vector2.Reflect(rb.linearVelocity, normal).normalized * bounceSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }
}
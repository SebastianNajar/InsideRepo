using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    // General
    public GameObject parryableProjectilePrefab;
    public Transform projectileSpawnPoint; // Where the parryable projectile spawns
    private int currentPhase = 1;
    private float phaseTimer = 0f;
    private bool isPhaseActive = true;
    private bool fightStarted = false; // NEW: Tracks if the fight has started
    private Collider2D bossCollider;


    // Phase 1
    public float phase1Duration = 15f;
    public float bounceSpeed = 5f;
    private Rigidbody2D rb;

    // Phase 2
    public GameObject residueProjectilePrefab;
    public float residueProjectileInterval = 2f; // Time between residue projectile shots
    private float residueTimer = 0f;

    // Phase 3
    public GameObject binaryProjectilePrefab;
    public float binaryProjectileInterval = 1f; // Time between binary projectile shots
    private float binaryTimer = 0f;

    //sprite
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        bossCollider = GetComponent<Collider2D>();
        if (bossCollider != null)
        {
            bossCollider.enabled = false;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false; // Hide the boss initially
        }

        rb = GetComponent<Rigidbody2D>();
        StartPhase1();
    }

    void Update()
    {
        if (!fightStarted) return;

        if (isPhaseActive)
        {
            switch (currentPhase)
            {
                case 1:
                    Phase1Behavior();
                    break;
                case 2:
                    Phase2Behavior();
                    break;
                case 3:
                    Phase3Behavior();
                    break;
            }
        }
    }

    public void StartFight()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }
        fightStarted = true;
        StartPhase1();
    }

    // Phase 1 Logic
    private void StartPhase1()
    {
        currentPhase = 1;
        isPhaseActive = true;
        phaseTimer = 0f;
        rb.linearVelocity = GetRandomBounceDirection() * bounceSpeed;
    }

    private void Phase1Behavior()
    {
        // Handle bouncing
        if (rb.linearVelocity.magnitude < 0.1f) // Reapply random velocity if it stops
        {
            rb.linearVelocity = GetRandomBounceDirection() * bounceSpeed;
        }

        // Update phase timer
        phaseTimer += Time.deltaTime;

        // Launch parryable projectile after 15 seconds
        if (phaseTimer >= phase1Duration)
        {
            LaunchParryableProjectile();
            isPhaseActive = false; // Temporarily stop behavior until parry resolves
        }
    }

    // Phase 2 Logic
    private void StartPhase2()
    {
        currentPhase = 2;
        isPhaseActive = true;
        phaseTimer = 0f;
        residueTimer = 0f;
        rb.linearVelocity = Vector2.zero; // Stop the boss's movement
    }

    private void Phase2Behavior()
    {
        phaseTimer += Time.deltaTime;
        residueTimer += Time.deltaTime;

        // Shoot residue projectiles at intervals
        if (residueTimer >= residueProjectileInterval)
        {
            residueTimer = 0f;
            Instantiate(residueProjectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        }

        // Launch parryable projectile after 15 seconds
        if (phaseTimer >= phase1Duration)
        {
            LaunchParryableProjectile();
            isPhaseActive = false; // Temporarily stop behavior until parry resolves
        }
    }

    // Phase 3 Logic
    private void StartPhase3()
    {
        currentPhase = 3;
        isPhaseActive = true;
        phaseTimer = 0f;
        residueTimer = 0f;
        binaryTimer = 0f;
        rb.linearVelocity = Vector2.zero; // Stop the boss's movement
    }

    private void Phase3Behavior()
    {
        phaseTimer += Time.deltaTime;
        residueTimer += Time.deltaTime;
        binaryTimer += Time.deltaTime;

        // Shoot residue projectiles at intervals
        if (residueTimer >= residueProjectileInterval)
        {
            residueTimer = 0f;
            Instantiate(residueProjectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        }

        // Shoot binary projectiles at intervals
        if (binaryTimer >= binaryProjectileInterval)
        {
            binaryTimer = 0f;
            Vector2 randomDirection = GetRandomDirection();
            Instantiate(binaryProjectilePrefab, projectileSpawnPoint.position, Quaternion.identity).GetComponent<Rigidbody2D>().linearVelocity = randomDirection * bounceSpeed;
        }

        // Launch parryable projectile after 15 seconds
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
        Instantiate(parryableProjectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
    }

    public void OnParrySuccess()
    {
        Debug.Log("Parry succeeded!");

        switch (currentPhase)
        {
            case 1:
                StartPhase2();
                break;
            case 2:
                StartPhase3();
                break;
            case 3:
                Die();
                break;
        }
    }

    public void OnParryFail()
    {
        Debug.Log("Parry failed!");
        isPhaseActive = true; // Restart current phase
        phaseTimer = 0f;
    }

    // Boss Death
    private void Die()
    {
        Debug.Log("Boss defeated!");
        Destroy(gameObject);
    }

    // Utility Methods
    private Vector2 GetRandomBounceDirection()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    private Vector2 GetRandomDirection()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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

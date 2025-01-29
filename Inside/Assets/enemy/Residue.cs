using UnityEngine;

public class Residue : MonoBehaviour
{
    public int damage = 1; // The damage the player will take
    public float damageInterval = 2f; // The time interval in seconds between taking damage

    private bool isPlayerOnResidue = false; // Whether the player is standing on the residue
    private float damageTimer = 0f; // Timer to track time for repeated damage

    public float explosionTimer;
    public GameObject explosion;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            isPlayerOnResidue = true; // Player is on the residue
            damageTimer = 0f; // Reset the damage timer when the player first steps on it
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player leaves the residue
        if (other.CompareTag("Player"))
        {
            isPlayerOnResidue = false; // Player leaves, stop the damage
        }
    }

    private void Update()
    {
        explosionTimer -= Time.deltaTime;

        if (explosionTimer <= 0f)
        {
            Explode();
        }
        // If the player is standing on the residue, apply damage over time
        if (isPlayerOnResidue)
        {
            damageTimer += Time.deltaTime; // Increment the timer based on time passed

            // If the timer exceeds the damage interval, apply damage and reset the timer
            if (damageTimer >= damageInterval)
            {
                damageTimer = 0f; // Reset the timer
                ApplyDamage(); // Damage the player
            }
        }
    }

    private void ApplyDamage()
    {
        // Find the player and apply damage
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }

    public void cleanResidue()
    {
        Debug.Log("Residue cleaned");
        Destroy(gameObject);
    }

    public void Explode()
    {
        GameObject obj = Instantiate(explosion, this.transform.position, this.transform.rotation);
        ApplyDamage();
        Destroy(gameObject);
    }
}
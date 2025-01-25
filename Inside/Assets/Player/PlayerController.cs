using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(1, 10)]
    public float moveSpeed;

    private Rigidbody2D RB;
    private Animator animator;
    public GameObject respawnPoint;

    public bool canMove = true;

    //interaction key
    public KeyCode interactKey = KeyCode.E;
    public float holdThreshold = 0.5f;
    private float interactTimer = 0f;
    private bool isHolding = false;

    //residue
    private GameObject currentResidue = null;
    //parry
    private GameObject nearbyProjectile = null;
    private float parryCooldown = 0f;

    //Get the required components from itself
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleInput();
        //Player Movement
        if (canMove)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector2 direction = new Vector2(horizontal * moveSpeed, vertical * moveSpeed);

            AnimateMovement(direction);
            RB.linearVelocity = RoundTo8Directions(direction);

            parryCooldown += Time.deltaTime;
        }
        if (canMove == false)
        {
            AnimateMovement(Vector3.zero);
            RB.linearVelocity = Vector3.zero;
        }

        

        
    }

    //Animate the player based on movement and direction
    public void AnimateMovement(Vector2 dir)
    {
        float horizontal = dir.x;
        float vertical = dir.y;
        animator.SetInteger("XDir", Mathf.Clamp((int)(Mathf.Sign(horizontal) * Mathf.Ceil(Mathf.Abs(horizontal))), -1, 1));
        animator.SetInteger("YDir", Mathf.Clamp((int)(Mathf.Sign(vertical) * Mathf.Ceil(Mathf.Abs(vertical))), -1, 1));
    }

    //Round the players movement to 8 directions
    public Vector2 RoundTo8Directions(Vector2 dir)
    {
        float angle = Vector2.SignedAngle(dir, Vector2.right);
        angle = -Mathf.Round(angle / 45f) * 45;
        Vector2 RoundedDirnew = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
        return RoundedDirnew * dir.magnitude;
    }

    //If the player dies reset them to last respawn point
    public void Death()
    {
        transform.position = respawnPoint.transform.position;
    }

    private void HandleInput()
    {
        // Check if the interact key is pressed
        if (Input.GetKeyDown(interactKey))
        {
            // Reset the timer and start tracking
            interactTimer = 0f;
            isHolding = true;
        }

        // Increment the timer while the key is held down
        if (Input.GetKey(interactKey) && isHolding)
        {
            interactTimer += Time.deltaTime;
            if (interactTimer >= holdThreshold)
            {
                CleanResidue();
                isHolding = false; 
            }
        }

        if (Input.GetKeyUp(interactKey))
        {
            if (isHolding && interactTimer < holdThreshold && parryCooldown > 2)
            {
                Parry();
                parryCooldown = 0;
            }
            isHolding = false;
            interactTimer = 0f;
        }
    }

     void CleanResidue()
    {
        Debug.Log("Cleaning residue...");
        if (currentResidue != null)
        {
            Debug.Log("Cleaning residue...");
            Destroy(currentResidue);

            // insert animation for cleaning residue?

            currentResidue = null; 
        }
        else
        {
            Debug.Log("No residue to clean!");
        }
    }
    void Parry()
    {
        Debug.Log("Tap action triggered...");
        if (nearbyProjectile != null)
        {
            ParryProjectile(nearbyProjectile);
            Debug.Log("parried");
        }
        else
        {
            Debug.Log("No projectile to parry!");
        }
    }



    private void ParryProjectile(GameObject projectile)
    {
        Debug.Log("Parried the projectile!");

        // Reverse the projectile's direction
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 reversedVelocity = -rb.linearVelocity; // Reverse the current velocity
            rb.linearVelocity = reversedVelocity;

            // Optional: Make the projectile hostile to enemies instead of the player
            ParryProjectile behavior = projectile.GetComponent<ParryProjectile>();
            if (behavior != null)
            {
                behavior.ChangeTargetToEnemies(); // Example method to make it harm enemies
            }
        }
    }



    //residue and projectile collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //residue
        if (collision.CompareTag("residue"))
        {
            currentResidue = collision.gameObject;
            Debug.Log("Residue in range!");
        }
        
        //projectile
        if (collision.CompareTag("parry"))
        {
            nearbyProjectile = collision.gameObject;
            Debug.Log("projectile is nearby and ready to parry");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //residue
        if (collision.CompareTag("residue"))
        {
            if (currentResidue == collision.gameObject)
            {
                currentResidue = null;
                Debug.Log("Residue out of range!");
            }
        }
        //projectile
        if (collision.CompareTag("parry"))
        {
            if (nearbyProjectile == collision.gameObject)
            {
                nearbyProjectile = null;
                Debug.Log("projectile out of range");
            }
        }
    }

}

 
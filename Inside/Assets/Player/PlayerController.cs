using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(1, 10)]
    public float moveSpeed;

    private Rigidbody2D RB;
    private Animator animator;
    public GameObject respawnPoint;

    public bool canMove = true;

    //residue cleaning
    public float interactionRadius = 1f;
    private GameObject nearbyResidue;
    public KeyCode interactKey = KeyCode.E;

    //Get the required components from itself
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //Player Movement
        if (canMove)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector2 direction = new Vector2(horizontal * moveSpeed, vertical * moveSpeed);

            AnimateMovement(direction);
            RB.linearVelocity = RoundTo8Directions(direction);
        }

        //residue cleaning
        if (Input.GetKeyDown(interactKey) && nearbyResidue != null)
        {
            Debug.Log("key pressed");
            // call cleanResidue function
            Residue residue = nearbyResidue.GetComponent<Residue>();
            if (residue != null)
            {
                residue.cleanResidue();
                
            }
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


    //residue logic
    void onTriggerEnter2D(Collider2D collision){
        //check if object is fire
        if (collision.CompareTag("residue"))
        {
            nearbyResidue = collision.gameObject;
        }
    }

    void onTriggerExit2D(Collider2D collision){
        //clear reference when player leaves trigger
        if (collision.CompareTag("residue"))
        {
            nearbyResidue = null;
        }
    }
}

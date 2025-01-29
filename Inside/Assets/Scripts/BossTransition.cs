using System.Collections;
using UnityEngine;

public class BossTransition : MonoBehaviour
{
    public Animator windowBar;
    public PlayerController playerController;
    public BossBehavior boss;

    IEnumerator Transition()
    {
        playerController.canMove = false;

        yield return new WaitForSeconds(5);

        windowBar.SetTrigger("Close");

        yield return new WaitForSeconds(3);

        playerController.canMove = true;
        boss.StartFight();

        Destroy(gameObject);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            StartCoroutine(Transition());
            
        }
    }
}

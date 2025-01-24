using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public int moveSpeed;
    public bool canMove = false;
    public bool switchToPlayer = false;
    public bool followPlayer = false;

    private void Update()
    {
        if (canMove)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                transform.position = new Vector3(target.position.x, target.position.y, -10);
                canMove = false;
            }
        }
        if (switchToPlayer)
        {
            target = GameObject.Find("Player").transform;
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, target.position.y, -10), moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                switchToPlayer = false;
                followPlayer = true;
            }
        }
        if (followPlayer)
        {
            transform.position = new Vector3(0, target.position.y, -10);
        }
    }

    public void MoveCamera(Transform newTarget)
    {
        switchToPlayer = false;
        followPlayer = false;
        target = newTarget;
        canMove = true;
    }

    public void SwitchToPlayer()
    {
        switchToPlayer = true;
    }
}

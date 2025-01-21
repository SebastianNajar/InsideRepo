using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public int moveSpeed;
    public bool canMove = false;

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
    }

    public void MoveCamera(Transform newTarget)
    {
        target = newTarget;
        canMove = true;
    }
}

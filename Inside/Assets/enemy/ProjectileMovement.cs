using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    private Vector3 targetPosition;
    public float radius;
    public float speed;
    public GameObject residue;

    void Start()
    {
        //currently this sets a random point anywhere around the enemy within given radius;
        // should be changed to where randomPoint is only on the player's side of the platform?
        // if so, these pool of points should be changed depending on what side the enemy is on?
        Vector2 randomPoint = Random.insideUnitCircle * radius;
        targetPosition = new Vector3(randomPoint.x, randomPoint.y, transform.position.z);

        //debug line
        Debug.DrawLine(transform.position, targetPosition, Color.red, 2f);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        //reached target pos:
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Destroy(gameObject);
            Instantiate(residue, targetPosition, Quaternion.identity);
        }
    }
}

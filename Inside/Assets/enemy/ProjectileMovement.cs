using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 targetPosition;
    public float radius;
    public float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 

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
            //to-do create "virus fire"
        }
    }
}

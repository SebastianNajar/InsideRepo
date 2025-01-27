using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    private Vector3 targetPosition;
    public float speed;
    public GameObject residue;

    // Parameters to restrict target points
    public float minXOffset = 1f;
    public float maxXOffset = 5f;
    public float minYOffset = -2f;
    public float maxYOffset = 2f;

    public LayerMask boundaryLayer; // Layer for the boundary objects

    void Start()
    {
        // Calculate valid points dynamically within bounds
        targetPosition = GetValidTargetPosition();

        // Debug line to visualize the trajectory
        Debug.DrawLine(transform.position, targetPosition, Color.red, 2f);
    }

    void Update()
    {
        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if the projectile reached the target
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Destroy(gameObject);

            // Only leave residue if the position is not within a boundary
            if (!IsInsideBoundary(targetPosition))
            {
                Instantiate(residue, targetPosition, Quaternion.identity); // Leave residue at the target
            }
        }
    }

    private Vector3 GetValidTargetPosition()
    {
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
        Vector3 randomPosition;

        do
        {
            // Generate a random position within the offsets
            float randomX = Random.Range(transform.position.x + minXOffset, transform.position.x + maxXOffset);
            float randomY = Random.Range(bottomLeft.y + minYOffset, topRight.y + maxYOffset);

            // Clamp the position within the camera bounds
            randomX = Mathf.Clamp(randomX, bottomLeft.x, topRight.x);
            randomY = Mathf.Clamp(randomY, bottomLeft.y, topRight.y);

            randomPosition = new Vector3(randomX, randomY, transform.position.z);
        }
        while (IsInsideBoundary(randomPosition)); // Retry if the point is inside a boundary

        return randomPosition;
    }

    private bool IsInsideBoundary(Vector3 position)
    {
        // Check if the position overlaps any boundary object
        Collider2D hitCollider = Physics2D.OverlapPoint(position, boundaryLayer);
        return hitCollider != null; // True if it hits a boundary
    }
}

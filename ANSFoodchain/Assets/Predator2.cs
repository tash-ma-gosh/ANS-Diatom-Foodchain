using UnityEngine;

public class Predator2 : MonoBehaviour
{
    public string playerTag = "Player";    // The tag of the player GameObject
    public float initialSpeed = 5f;        // The initial movement speed of the predator
    public float maxSpeed = 10f;           // The maximum movement speed of the predator
    public float accelerationRate = 2f;    // The rate at which the predator's speed increases
    public float minDistance = 2f;         // The minimum distance to keep from the target
    public float avoidanceRadius = 3f;     // The radius to avoid other predators

    private Rigidbody2D rb;
    private Collider2D[] nearbyColliders;

    private float currentSpeed;   // The current movement speed of the predator
    private Transform target;     // Reference to the player's transform

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        nearbyColliders = new Collider2D[10]; // Adjust the size as needed for the number of predators in the scene

        currentSpeed = initialSpeed;

        FindPlayer();
    }

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            target = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player object not found!");
        }
    }

    private void Update()
    {
        if (target == null)
            return;

        // Calculate the direction towards the target
        Vector2 direction = target.position - transform.position;
        direction.Normalize();

        // Check for nearby predators
        int numColliders = Physics2D.OverlapCircleNonAlloc(transform.position, avoidanceRadius, nearbyColliders);

        // Avoid other predators
        Vector2 avoidanceMove = Vector2.zero;
        for (int i = 0; i < numColliders; i++)
        {
            if (nearbyColliders[i] != null && nearbyColliders[i].gameObject != gameObject)
            {
                Vector2 avoidDirection = (Vector2)transform.position - (Vector2)nearbyColliders[i].transform.position;
                avoidanceMove += avoidDirection.normalized;
            }
        }

        // Move away from nearby predators
        if (numColliders > 0)
            direction += avoidanceMove.normalized;

        // Move towards the target with speed over time
        currentSpeed = Mathf.Min(currentSpeed + accelerationRate * Time.deltaTime, maxSpeed);
        Vector2 targetPosition = (Vector2)transform.position + direction * currentSpeed * Time.deltaTime;

        // Perform collision detection and resolution
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, targetPosition - (Vector2)transform.position, (targetPosition - (Vector2)transform.position).magnitude);
        bool collided = false;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject != gameObject)
            {
                collided = true;
                break;
            }
        }

        if (!collided)
            rb.MovePosition(targetPosition);

        // Manually rotate towards the target direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}

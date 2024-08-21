using UnityEngine;

// This script controls the movement of a target in the game.
public class TargetMover : MonoBehaviour
{
    public float speed = 2f; // Speed of movement of the target
    public float sidewaysDistance = 0f; // Distance the target moves sideways
    public float forwardDistance = 0f; // Distance the target moves forward and backward

    private Vector3 startPosition; // Initial position of the target
    private Vector3 targetPosition; // Position the target moves towards
    private bool movingToTarget = true; // Flag to track movement direction

    void Start()
    {
        startPosition = transform.position; // Store the initial position
        targetPosition = startPosition + new Vector3(sidewaysDistance, 0, forwardDistance); // Calculate the target position
    }

    void Update()
    {
        float step = speed * Time.deltaTime; // Calculate the step size based on speed and time
        if (movingToTarget)
        {
            // Move towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                movingToTarget = false; // Switch direction when target is reached
            }
        }
        else
        {
            // Move back to the start position
            transform.position = Vector3.MoveTowards(transform.position, startPosition, step);
            if (Vector3.Distance(transform.position, startPosition) < 0.001f)
            {
                movingToTarget = true; // Switch direction when start is reached
            }
        }
    }
}
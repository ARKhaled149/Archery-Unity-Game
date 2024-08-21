using System.Collections;
using UnityEngine;

// This script handles the behavior of an arrow, including movement and collision detection.
public class Arrow : MonoBehaviour
{
    public float speed = 10f; // Speed at which the arrow travels
    public Transform tip; // The tip of the arrow used for collision detection

    private Rigidbody _rigidBody; // Rigidbody component for physics calculations
    private bool _inAir = false; // Flag indicating whether the arrow is in motion
    private Vector3 _lastPosition = Vector3.zero; // Track the last position for collision detection
    private GameManager gameManager; // Reference to the GameManager to handle score and arrows

    public AudioSource hitSound; // Audio played when the arrow hits a target
    public ParticleSystem FireEffect; // Visual effect played on impact

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>(); // Get the Rigidbody component
        PullInteraction.PullActionReleased += Release; // Subscribe to the release event

        gameManager = FindObjectOfType<GameManager>(); // Find the GameManager in the scene
        Stop(); // Initialize the arrow's state
    }

    private void OnDestroy()
    {
        PullInteraction.PullActionReleased -= Release; // Unsubscribe from the event to prevent memory leaks
    }

    public void Release(float pullValue)
    {
        if (gameManager.HasArrowsLeft())
        {
            PullInteraction.PullActionReleased -= Release; // Unsubscribe to prevent multiple calls
            gameObject.transform.parent = null; // Detach from the parent object
            _inAir = true; // Set the arrow as in motion
            SetPhysics(true); // Enable physics interactions

            Vector3 force = transform.forward * pullValue * speed; // Calculate the force based on pull
            _rigidBody.AddForce(force, ForceMode.Impulse); // Apply force to the arrow

            StartCoroutine(RotateWithVelocity()); // Start rotating arrow with velocity

            _lastPosition = tip.position; // Update the last position

            gameManager.UseArrow(); // Inform GameManager an arrow was used
        }
    }

    private IEnumerator RotateWithVelocity()
    {
        yield return new WaitForFixedUpdate(); // Wait for the next physics update
        while (_inAir)
        {
            if (_rigidBody.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                Quaternion newRotation = Quaternion.LookRotation(_rigidBody.velocity, transform.up); // Rotate to follow velocity
                transform.rotation = newRotation;
            }
            yield return null; // Continue in the next frame
        }
    }

    private void FixedUpdate()
    {
        if (_inAir)
        {
            CheckCollision(); // Check for collisions while in air
            _lastPosition = tip.position; // Update the last position
        }
    }

    private void CheckCollision()
    {
        if (Physics.Linecast(_lastPosition, tip.position, out RaycastHit hitInfo))
        {
            // Handle collision with objects
            if (hitInfo.transform.TryGetComponent(out Rigidbody body))
            {
                _rigidBody.interpolation = RigidbodyInterpolation.None; // Disable interpolation
                transform.SetParent(hitInfo.transform); // Attach arrow to hit object
                body.AddForce(_rigidBody.velocity, ForceMode.Impulse); // Apply force to hit object
            }

            if (hitSound != null)
            {
                hitSound.Play(); // Play impact sound
            }

            if (FireEffect != null)
            {
                FireEffect.Play(); // Trigger visual effects
            }

            // Check target tag and update score
            switch (hitInfo.transform.tag)
            {
                case "LowPointsTargets":
                    gameManager.AddScore(5); // Add points for low-value target
                    break;
                case "MediumPointsTargets":
                    gameManager.AddScore(10); // Add points for medium-value target
                    break;
                case "HighPointsTargets":
                    gameManager.AddScore(15); // Add points for high-value target
                    break;
                case "VeryHighPointsTargets":
                    gameManager.AddScore(25); // Add points for very high-value target
                    Debug.Log("VeryHigh");
                    break;
            }

            Stop(); // Stop the arrow after collision
        }
    }

    private void Stop()
    {
        _inAir = false; // Mark the arrow as not in motion
        SetPhysics(false); // Disable physics interactions
    }

    private void SetPhysics(bool usePhysics)
    {
        _rigidBody.useGravity = usePhysics; // Enable or disable gravity
        _rigidBody.isKinematic = !usePhysics; // Set kinematic state based on usePhysics
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// This script manages spawning arrows on the bow for the player to shoot.
public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab; // Prefab used to instantiate new arrows
    public Transform attachPoint; // Point on the bow where arrows are attached

    private XRGrabInteractable bowInteractable; // Interactable component of the bow
    private bool arrowNotched = false; // Flag to track if the arrow is notched
    private GameObject currentArrow; // Reference to the currently notched arrow
    private GameManager gameManager; // Reference to the GameManager

    public AudioSource shootSound; // Sound played when an arrow is shot

    void Start()
    {
        bowInteractable = GetComponentInParent<XRGrabInteractable>(); // Get the interactable component from the parent bow
        gameManager = FindObjectOfType<GameManager>(); // Find the GameManager in the scene
        PullInteraction.PullActionReleased += ReleaseArrow; // Subscribe to the release event
    }

    private void OnDestroy()
    {
        PullInteraction.PullActionReleased -= ReleaseArrow; // Unsubscribe from the event to prevent memory leaks
    }

    void Update()
    {
        // Check if the bow is held, an arrow is not yet notched, and there are arrows left
        if (bowInteractable.isSelected && !arrowNotched && gameManager.HasArrowsLeft() && ArrowDestroyer.canSpawn > 0)
        {
            arrowNotched = true; // Mark the arrow as notched
            StartCoroutine(DelayedSpawn()); // Delay the spawning of the arrow
        }

        // If the bow is released, destroy the current arrow
        if (!bowInteractable.isSelected && currentArrow != null)
        {
            Destroy(currentArrow); // Destroy the currently notched arrow
            NotchEmpty(); // Reset the notch state
        }
    }

    private void NotchEmpty()
    {
        arrowNotched = false; // Mark the notch as empty
        currentArrow = null; // Clear the reference to the current arrow
    }

    private void ReleaseArrow(float pullValue)
    {
        if (currentArrow != null)
        {
            // Detach the arrow and apply force
            ArrowDestroyer.canSpawn -= 1; // Decrement the count of arrows that can be spawned
            Debug.Log("canSpawn = " + ArrowDestroyer.canSpawn);
            currentArrow.transform.SetParent(null); // Detach the arrow from the bow
            Rigidbody arrowRigidbody = currentArrow.GetComponent<Rigidbody>();
            arrowRigidbody.isKinematic = false; // Enable physics
            arrowRigidbody.useGravity = true; // Enable gravity
            arrowRigidbody.AddForce(attachPoint.forward * pullValue * currentArrow.GetComponent<Arrow>().speed, ForceMode.Impulse); // Apply force for shooting

            if (shootSound != null)
            {
                shootSound.Play(); // Play the shooting sound
            }

            // Reset the state
            NotchEmpty(); // Mark the notch as empty
        }
    }

    private IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(1f); // Wait for a second before spawning the arrow

        // Instantiate the arrow at the attach point position
        currentArrow = Instantiate(arrowPrefab, attachPoint.position, attachPoint.rotation);
        currentArrow.transform.SetParent(attachPoint); // Attach the arrow to the bow
        Rigidbody arrowRigidbody = currentArrow.GetComponent<Rigidbody>();
        arrowRigidbody.isKinematic = true; // Disable physics while attached
        arrowRigidbody.useGravity = false; // Disable gravity while notched
    }
}
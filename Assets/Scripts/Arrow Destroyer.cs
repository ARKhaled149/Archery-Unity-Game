using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// This script manages the destruction of arrows when they are no longer needed.
public class ArrowDestroyer : MonoBehaviour
{
    public XRGrabInteractable arrowInteractable; // Reference to the interactable component of the arrow
    public XRGrabInteractable bowInteractable; // Reference to the interactable component of the bow
    public static int canSpawn = 0; // Counter to control arrow spawning

    // Update is called once per frame
    void Update()
    {
        // Check if both the bow and arrow are selected and an arrow can be spawned
        if (bowInteractable.isSelected && arrowInteractable.isSelected && canSpawn == 0)
        {
            canSpawn += 1; // Increment the spawn counter
            Debug.Log(canSpawn); // Log the current spawn count
            Destroy(gameObject); // Destroy this arrow object
        }
    }
}
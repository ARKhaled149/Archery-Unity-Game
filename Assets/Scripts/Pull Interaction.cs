using System;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

// This script handles the interaction of pulling a bowstring in a VR environment.
public class PullInteraction : XRBaseInteractable
{
    public static event Action<float> PullActionReleased; // Event triggered when the pull action is released
    public Transform start, end; // Transforms defining the start and end points of the pull
    public GameObject notch; // Reference to the notch object
    public float pullAmount { get; private set; } = 0.0f; // Amount of pull, ranging from 0 to 1
    private LineRenderer _lineRenderer; // Line renderer to visualize the string
    private IXRSelectInteractor pullingInteractor = null; // The interactor currently pulling the string

    public AudioSource drawSound; // Sound played when the bowstring is drawn

    protected override void Awake()
    {
        base.Awake();
        _lineRenderer = GetComponent<LineRenderer>(); // Initialize the line renderer
    }

    public void SetPullInteractor(SelectEnterEventArgs args)
    {
        pullingInteractor = args.interactorObject; // Set the interactor that is pulling

        if (drawSound != null)
        {
            drawSound.Play(); // Play the draw sound if available
        }
    }

    public void Release()
    {
        PullActionReleased?.Invoke(pullAmount); // Trigger the release event with the current pull amount
        pullingInteractor = null; // Reset the interactor
        pullAmount = 0f; // Reset the pull amount
        notch.transform.localPosition = new Vector3(notch.transform.localPosition.x, notch.transform.localPosition.y, 0f);
        UpdateString(); // Update the visual representation of the string
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (isSelected && pullingInteractor != null)
            {
                Vector3 pullPosition = pullingInteractor.transform.position;
                pullAmount = CalculatePull(pullPosition); // Calculate the amount of pull
                UpdateString(); // Update the string visualization
            }
        }
    }

    private float CalculatePull(Vector3 pullPosition)
    {
        Vector3 pullDirection = pullPosition - start.position;
        Vector3 targetDirection = end.position - start.position;
        float maxLength = targetDirection.magnitude;
        targetDirection.Normalize();
        float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength; // Calculate pull value based on direction
        return Mathf.Clamp(pullValue, 0, 1); // Clamp the value between 0 and 1
    }

    private void UpdateString()
    {
        Vector3 linePosition = Vector3.forward * Mathf.Lerp(start.transform.localPosition.z, end.transform.localPosition.z, pullAmount);
        notch.transform.localPosition = new Vector3(notch.transform.localPosition.x, notch.transform.localPosition.y, linePosition.z + .2f);
        _lineRenderer.SetPosition(1, linePosition); // Update the line renderer position to visualize the string
    }
}
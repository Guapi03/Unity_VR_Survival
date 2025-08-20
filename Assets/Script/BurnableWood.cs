using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BurnableWood : MonoBehaviour
{
    public bool isBurning = false;

    public void StartBurning()
    {
        isBurning = true;

        XRGrabInteractable grab = GetComponent<XRGrabInteractable>();
        if (grab != null)
        {
            grab.enabled = false; // Prevent further grabbing
        }

        // Optional: Change material or VFX to show it�s�burning
    }
}
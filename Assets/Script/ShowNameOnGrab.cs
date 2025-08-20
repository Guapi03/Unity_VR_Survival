using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShowNameOnGrab : MonoBehaviour
{
    [Header("Item Info")]
    public string itemName = "Unnamed Object"; 

    private XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        if (grab != null)
        {
            grab.selectEntered.AddListener(OnGrabbed);
            grab.selectExited.AddListener(OnReleased);
        }
    }

    void OnGrabbed(SelectEnterEventArgs args)
    {
        UnityEngine.Debug.Log("Picked up: " + itemName);
    }

    void OnReleased(SelectExitEventArgs args)
    {
        UnityEngine.Debug.Log("Released: " + itemName);
    }
}
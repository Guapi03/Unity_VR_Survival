using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StorableItem : MonoBehaviour
{
    public string itemName = "Unnamed Item";

    private XRGrabInteractable grab;

    private bool isHeld = false;
    private XRBaseInteractor currentInteractor;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();

        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        isHeld = true;
        currentInteractor = args.interactor;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false;
        currentInteractor = null;
    }

    public bool IsHeldByLeftHand()
    {
        return isHeld && currentInteractor != null && currentInteractor.name.Contains("Left");
    }
}
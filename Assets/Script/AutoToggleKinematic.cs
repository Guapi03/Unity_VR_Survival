using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AutoToggleKinematic : MonoBehaviour
{
    private Rigidbody rb;
    private XRGrabInteractable grab;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grab = GetComponent<XRGrabInteractable>();

        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        rb.isKinematic = true;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        rb.isKinematic = false;
    }
}
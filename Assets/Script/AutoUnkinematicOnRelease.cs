using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class AutoUnkinematicOnRelease : MonoBehaviour
{
    private XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectExited.AddListener(OnReleased);
    }

    void OnReleased(SelectExitEventArgs args)
    {
        StartCoroutine(UnKinematicNextFrame());
    }

    IEnumerator UnKinematicNextFrame()
    {
        yield return null; // wait 1 frame
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }
    }
}
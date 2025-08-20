using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InventoryItemTracker : MonoBehaviour
{
    public int slotIndex = -1;
    public WristInventory inventory;

    private XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        if (grab != null)
        {
            grab.selectEntered.AddListener(OnGrabbed);
        }
    }

    void OnGrabbed(SelectEnterEventArgs args)
    {
        if (inventory != null && slotIndex >= 0)
        {
            // Remove from inventory
            inventory.RemoveItem(slotIndex);
            slotIndex = -1;

            // Force scale restore in next frame
            StartCoroutine(RestoreScaleNextFrame());
        }
    }

    IEnumerator RestoreScaleNextFrame()
    {
        yield return null; // wait 1 frame

    }
}
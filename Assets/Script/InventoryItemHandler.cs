using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InventoryItemHandler : MonoBehaviour
{
    private XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectExited.AddListener(OnReleased);
    }

    void OnReleased(SelectExitEventArgs args)
    {
        InventoryItem item = GetComponent<InventoryItem>();

        // Already in slot, do nothing
        if (item != null && item.inSlot) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, 0.05f);

        foreach (Collider hit in hits)
        {
            Slot slot = hit.GetComponent<Slot>();
            if (slot != null && slot.ItemInSlot == null)
            {
                slot.InsertItem(gameObject);
                break;
            }
        }
    }
}
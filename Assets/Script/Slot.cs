using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class Slot : MonoBehaviour
{
    public GameObject ItemInSlot;
    public Image slotImage;
    XRGrabInteractable grab;
    private Color originalColor;

    void Start()
    {
        originalColor = slotImage.color;
    }

    private void OnTriggerStay(Collider other)
    {
        if (ItemInSlot != null) return;

        GameObject obj= other.gameObject;
        Debug.Log("trigger dectected :" + obj.name);
        InventoryItem item = other.GetComponent<InventoryItem>();
        XRGrabInteractable grab = other.GetComponent<XRGrabInteractable>();

        if (item == null || grab == null) return;

        // Check if it's no longer grabbed
        if (!grab.isSelected)
        {
            InsertItem(other.gameObject);
        }
    }

    public void InsertItem(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;

        obj.transform.SetParent(transform, true);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localEulerAngles = obj.GetComponent<InventoryItem>().slotRotation;

        var item = obj.GetComponent<InventoryItem>();
        item.inSlot = true;
        item.currentSlot = this;

        ItemInSlot = obj;
        slotImage.color = Color.gray;
    }

    public void ResetColor()
    {
        slotImage.color = originalColor;
    }
}
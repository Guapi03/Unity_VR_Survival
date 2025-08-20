using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WristInventory : MonoBehaviour
{
    public Transform[] slotParents = new Transform[8];
    public GameObject[] currentIcons = new GameObject[8];
    public GameObject[] originalPrefabs = new GameObject[8];

    public XRInteractionManager interactionManager; // 管理器引用

    public bool TryStoreItem(GameObject item)
    {
        for (int i = 0; i < slotParents.Length; i++)
        {
            if (currentIcons[i] == null)
            {
                StoreInSlot(i, item);
                return true;
            }
        }

        Debug.Log("❌ Inventory full!");
        return false;
    }

    void StoreInSlot(int index, GameObject originalItem)
    {
        InventoryItemInfo info = originalItem.GetComponent<InventoryItemInfo>();
        if (info == null || info.iconPrefab == null || info.originalPrefab == null) return;

        GameObject icon = Instantiate(info.iconPrefab, slotParents[index]);
        icon.transform.localPosition = Vector3.zero;
        icon.transform.localRotation = Quaternion.identity;
        icon.transform.localScale = Vector3.one;

        currentIcons[index] = icon;
        originalPrefabs[index] = info.originalPrefab;

        Destroy(originalItem);
    }

    // ✅ 主函数：生成 item 并放进玩家手中


    public void SpawnAndGrabItem(int index, XRBaseInteractor interactor)
    {
        if (originalPrefabs[index] == null || currentIcons[index] == null)
        {
            Debug.LogWarning($"❌ Slot {index + 1} is empty or data missing.");
            return;
        }

        // Spawn the real item
        Vector3 spawnPos = slotParents[index].position + Vector3.forward * 0.2f;
        GameObject spawned = Instantiate(originalPrefabs[index], spawnPos, Quaternion.identity);

        // Ensure it's interactable
        XRGrabInteractable grabItem = spawned.GetComponent<XRGrabInteractable>();
        if (grabItem && interactionManager != null)
        {
            interactionManager.SelectEnter(interactor, grabItem);
        }

        // ✅ Clean up this slot completely
        Destroy(currentIcons[index]);
        currentIcons[index] = null;
        originalPrefabs[index] = null;

        Debug.Log($"🪓 Item from slot {index + 1} spawned and grabbed.");
    }

    public GameObject TakeItem(int index)
    {
        if (currentIcons[index] == null || originalPrefabs[index] == null)
        {
            Debug.Log("❌ No item in this slot!");
            return null;
        }

        // Spawn item in front of slot
        Vector3 spawnPos = slotParents[index].position + slotParents[index].forward * 0.2f;
        GameObject spawned = Instantiate(originalPrefabs[index], spawnPos, Quaternion.identity);

        // Setup Rigidbody to prevent weird physical behavior
        Rigidbody rb = spawned.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }

        // Make sure Collider is enabled
        Collider col = spawned.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        // Optional: reset layer to "Interactable"
        spawned.layer = LayerMask.NameToLayer("Default");

        // Cleanup icon & inventory slot
        Destroy(currentIcons[index]);
        currentIcons[index] = null;
        originalPrefabs[index] = null;

        return spawned;
    }

    public void RemoveItem(int index)
    {
        // Make sure index is valid
        if (index < 0 || index >= currentIcons.Length)
        {
            Debug.LogWarning("❌ Invalid inventory slot index.");
            return;
        }

        if (currentIcons[index] == null)
        {
            Debug.Log("ℹ No item to remove in slot " + (index + 1));
            return;
        }

        Destroy(currentIcons[index]);
        currentIcons[index] = null;
        originalPrefabs[index] = null;

        Debug.Log($"🗑 Removed item from slot {index + 1}");
    }
}



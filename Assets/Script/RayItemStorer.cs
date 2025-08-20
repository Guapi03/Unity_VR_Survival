using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RayItemStorer : MonoBehaviour
{
    public InputActionReference storeAction; // Bind to X Button
    public XRRayInteractor rayInteractor;    // Left hand ray interactor
    public WristInventory wristInventory;

    void OnEnable()
    {
        storeAction.action.performed += TryStoreItem;
        storeAction.action.Enable();
    }

    void OnDisable()
    {
        storeAction.action.performed -= TryStoreItem;
        storeAction.action.Disable();
    }

    void TryStoreItem(InputAction.CallbackContext context)
    {
        if (rayInteractor.selectTarget == null) return;

        GameObject heldItem = rayInteractor.selectTarget.gameObject;

        bool success = wristInventory.TryStoreItem(heldItem);
        if (success)
        {
            Debug.Log($"✅ Stored {heldItem.name} via X button.");
        }
        else
        {
            Debug.Log("❌ Inventory is full.");
        }
    }
}
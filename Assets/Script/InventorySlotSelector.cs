using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class InventorySlotSelector : MonoBehaviour
{
    public WristInventory wristInventory;
    public XRRayInteractor rayInteractor;
    public InputActionReference triggerAction; // Right controller trigger

    private void OnEnable()
    {
        triggerAction.action.performed += OnTriggerPressed;
        triggerAction.action.Enable();
    }

    private void OnDisable()
    {
        triggerAction.action.performed -= OnTriggerPressed;
        triggerAction.action.Disable();
    }

    void OnTriggerPressed(InputAction.CallbackContext ctx)
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            Transform hitSlot = hit.transform;

            // Check which slot was hit
            for (int i = 0; i < wristInventory.slotParents.Length; i++)
            {
                if (hitSlot == wristInventory.slotParents[i])
                {
                    wristInventory.TakeItem(i);
                    Debug.Log($"🎒 Grabbed item from slot {i + 1}");
                    break;
                }
            }
        }
    }
}
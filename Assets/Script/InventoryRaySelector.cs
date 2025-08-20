using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class InventoryRaySelector : MonoBehaviour
{
    public WristInventory wristInventory;
    public XRRayInteractor rayInteractor;
    public InputActionReference selectAction; // A button
    public XRBaseInteractor rightHandInteractor; // 通常是右手 Ray 本身

    void OnEnable()
    {
        selectAction.action.performed += OnButtonPressed;
        selectAction.action.Enable();
    }

    void OnDisable()
    {
        selectAction.action.performed -= OnButtonPressed;
        selectAction.action.Disable();
    }

    void OnButtonPressed(InputAction.CallbackContext ctx)
    {
        if (!rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit)) return;

        Transform hitTransform = hit.transform;

        for (int i = 0; i < wristInventory.slotParents.Length; i++)
        {
            if (wristInventory.slotParents[i] == hitTransform)
            {
                wristInventory.SpawnAndGrabItem(i, rightHandInteractor);
                Debug.Log($"🧤 Grabbed item from slot {i + 1}");
                break;
            }




        }
    }
}
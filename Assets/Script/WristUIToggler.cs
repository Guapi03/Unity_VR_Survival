using UnityEngine;
using UnityEngine.InputSystem;

public class WristUIToggler : MonoBehaviour
{
    public GameObject wristUI; // Assign the UI Canvas here
    public InputActionReference toggleAction; // Reference to "X" button (or any)

    private bool isVisible = false;

    void Start()
    {
        // Start with UI hidden
        wristUI.SetActive(false);
        isVisible = false;
    }

    void OnEnable()
    {
        toggleAction.action.performed += ToggleUI;
    }

    void OnDisable()
    {
        toggleAction.action.performed -= ToggleUI;
    }

    void ToggleUI(InputAction.CallbackContext ctx)
    {
        isVisible = !isVisible;
        wristUI.SetActive(isVisible);
    }
}
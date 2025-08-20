using UnityEngine;
using UnityEngine.InputSystem;

public class GuideUIToggler : MonoBehaviour
{
    public GameObject wristUI; // Assign the UI Canvas here
    public InputActionReference toggleAction; // Reference to "X" button (or any)

    private bool isVisible = true;

    void Start()
    {
        // Start with UI hidden
        wristUI.SetActive(true);
        isVisible = true;
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
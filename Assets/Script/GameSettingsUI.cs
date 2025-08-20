using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameSettingsUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject settingsPanel;

    [Header("Input")]
    public InputActionReference toggleSettingsAction; // Bind to Y button (Left controller)

    private bool isPaused = false;

    void OnEnable()
    {
        settingsPanel.SetActive(false); // Hide UI on start

        if (toggleSettingsAction != null)
        {
            toggleSettingsAction.action.performed += OnTogglePressed;
            toggleSettingsAction.action.Enable();
        }
    }

    void OnDisable()
    {
        if (toggleSettingsAction != null)
        {
            toggleSettingsAction.action.performed -= OnTogglePressed;
            toggleSettingsAction.action.Disable();
        }
    }

    private void OnTogglePressed(InputAction.CallbackContext ctx)
    {
        ToggleSettings();
    }

    public void ToggleSettings()
    {
        isPaused = !isPaused;

        settingsPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
        AudioListener.pause = isPaused;

        Debug.Log(isPaused ? "⏸ Game Paused" : "▶ Game Resumed");
    }

    public void ResumeGame()
    {
        isPaused = false;
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;

        Debug.Log("▶ Game Resumed from Resume Button");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Quit for editor
#else
        Application.Quit(); // Quit for build
#endif
        Debug.Log("🚪 Quit Game triggered");
    }
}
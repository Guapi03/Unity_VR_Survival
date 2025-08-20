using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Scene 1"); 
    }

    public void QuitGame()
    {
        Debug.Log("🛑 Quit Game called");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#else
        Application.Quit(); 
#endif
    }
}
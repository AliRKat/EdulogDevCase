using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Start game function
    public void StartGame()
    {
        Debug.Log("Starting game...");
        SceneManager.LoadScene("GameScene");
    }

    // Open settings function
    public void OpenSettings()
    {
        Debug.Log("Opening settings...");
        // Þimdilik sadece log atýyor, buraya settings menüsünü açacak kod eklenebilir.
    }

    // Exit game function
    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
#if UNITY_EDITOR
        // Oyun Unity Editor içindeyse, editörden çýkmaz ama bunu loglar.
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
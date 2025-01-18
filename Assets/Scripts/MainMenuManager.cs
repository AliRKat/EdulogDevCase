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
        // �imdilik sadece log at�yor, buraya settings men�s�n� a�acak kod eklenebilir.
    }

    // Exit game function
    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
#if UNITY_EDITOR
        // Oyun Unity Editor i�indeyse, edit�rden ��kmaz ama bunu loglar.
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
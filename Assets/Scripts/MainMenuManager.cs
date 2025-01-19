using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject settings;
    [SerializeField] private Toggle soundToggle;
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    private void Start()
    {
        soundToggle.isOn = SoundManager.Instance.IsSoundEnabled();
        soundToggle.onValueChanged.AddListener(OnSoundToggleChanged);
    }

    private void OnDestroy()
    {
        soundToggle.onValueChanged.RemoveListener(OnSoundToggleChanged);
    }

    private void OnSoundToggleChanged(bool isOn)
    {
        SoundManager.Instance.SetSoundEnabled(isOn);
    }

    public void Settings()
    {
        if (settings != null)
        {
            bool isActive = settings.activeSelf;
            settings.SetActive(!isActive);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioSource backgroundMusicSource;

    private bool isSoundEnabled = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBackgroundMusic(AudioClip clip)
    {
        if (backgroundMusicSource.clip != clip)
        {
            backgroundMusicSource.clip = clip;
            backgroundMusicSource.loop = true;
        }

        if (isSoundEnabled && !backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        if (backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Stop();
        }
    }

    public void SetSoundEnabled(bool isEnabled)
    {
        isSoundEnabled = isEnabled;

        if (isEnabled)
        {
            backgroundMusicSource.Play();
        }
        else
        {
            backgroundMusicSource.Stop();
        }
    }

    public bool IsSoundEnabled()
    {
        return isSoundEnabled;
    }
}
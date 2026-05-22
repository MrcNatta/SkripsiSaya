using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;         // Untuk musik background (MainMenu, Gameplay, Ending)
    public AudioSource ambientSource;       // Untuk ambient loop gameplay

    [Header("Musik Background")]
    public AudioClip mainMenuMusic;         // MainMenu.mp3
    public AudioClip gameplayMusic;         // GameplayMusic.mp3
    public AudioClip ending1Music;          // Ending1.mp3
    public AudioClip ending2Music;          // Ending2.mp3
    public AudioClip ending3Music;          // Ending3.mp3

    [Header("Pengaturan Volume")]
    [Range(0f, 1f)] public float musicVolume = 0.5f;
    [Range(0f, 1f)] public float ambientVolume = 0.3f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Tetap hidup saat pindah scene
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Terapkan volume tersimpan dari VolumeControl
        float saved = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioListener.volume = saved;

        // Auto-detect scene saat pertama kali jalan
        PlayMusicForCurrentScene();
    }

    void OnEnable()
    {
        // Dengarkan event setiap kali scene berpindah
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Otomatis dipanggil setiap kali scene baru selesai di-load
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForCurrentScene();
    }

    private void PlayMusicForCurrentScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "MainMenu":
                PlayMusic(mainMenuMusic);
                StopAmbient();
                break;

            case "Level 1":
            case "Level 1A":
            case "Level 1B":
            case "Level 1C":
            case "Level 2A":
            case "Level 2B":
            case "Level 2C":
                PlayMusic(gameplayMusic);
                break;

            case "Ending1":
                PlayMusic(ending1Music);
                StopAmbient();
                break;

            case "Ending2":
                PlayMusic(ending2Music);
                StopAmbient();
                break;

            case "Ending3":
                PlayMusic(ending3Music);
                StopAmbient();
                break;

            case "LoadingScreen":
                // Biarkan musik yang sedang main terus — jangan ganti saat loading
                break;

            default:
                break;
        }
    }

    // =============================================
    // Fungsi internal musik
    // =============================================
    private void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        if (musicSource == null) return;

        // Jika musik yang sama sudah bermain, jangan restart
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void PlayAmbient(AudioClip clip)
    {
        if (clip == null) return;
        if (ambientSource == null) return;

        ambientSource.clip = clip;
        ambientSource.loop = true;
        ambientSource.volume = ambientVolume;
        ambientSource.Play();
    }

    public void StopAmbient()
    {
        if (ambientSource != null && ambientSource.isPlaying)
            ambientSource.Stop();
    }

    public void StopAll()
    {
        if (musicSource != null) musicSource.Stop();
        if (ambientSource != null) ambientSource.Stop();
    }

    // Dipanggil oleh VolumeControl saat slider digeser
    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        if (musicSource != null)
            musicSource.volume = value;
        if (ambientSource != null)
            ambientSource.volume = value * ambientVolume;
    }
}

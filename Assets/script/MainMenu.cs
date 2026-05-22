using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject settingsPanel;
    public GameObject howToPlayPanel;

    [Header("Volume")]
    public Slider volumeSlider;

    void Start()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (howToPlayPanel != null) howToPlayPanel.SetActive(false);

        // Ambil volume tersimpan — key "MasterVolume" konsisten dengan VolumeControl.cs
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioListener.volume = savedVolume;

        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
    }

    void OnDestroy()
    {
        if (volumeSlider != null)
            volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
    }

    // =====================
    // Tombol Main Menu
    // =====================

    public void PlayGame()
    {
        LoadingScreenManager.targetScene = "Level 1A";
        SceneManager.LoadScene("LoadingScreen");
    }

    public void OpenHowToPlay()
    {
        if (howToPlayPanel != null)
            howToPlayPanel.SetActive(true);
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // =====================
    // Tombol dalam Panel
    // =====================

    public void CloseHowToPlay()
    {
        if (howToPlayPanel != null)
            howToPlayPanel.SetActive(false);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    // =====================
    // Volume
    // =====================

    public void OnVolumeChanged(float value)
    {
        // Terapkan langsung ke AudioListener
        AudioListener.volume = value;

        // Sync ke AudioManager kalau ada (untuk musik)
        if (AudioManager.instance != null)
            AudioManager.instance.SetMusicVolume(value);

        // Simpan dengan key yang sama dengan VolumeControl.cs
        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();
    }
}
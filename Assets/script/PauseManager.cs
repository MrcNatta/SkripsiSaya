using UnityEngine;
using UnityEngine.SceneManagement;  // Untuk pindah ke scene Main Menu
using UnityEngine.Audio;            // Untuk AudioMixer (volume)

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;    // Singleton — bisa diakses dari mana saja
                                            // dengan PauseManager.instance
    [Header("UI Elements")]
    public GameObject pauseButton;          // Drag tombol pause di sini (opsional)
    [Header("Panels")]
    public GameObject pausePanel;           // Drag PausePanel ke sini
    public GameObject settingsPanel;        // Drag SettingsPanel ke sini
    public GameObject howToPlayPanel;       // Drag HowToPlayPanel ke sini
    public GameObject howToPlayPanel2;      // Drag HowToPlayPanel2 ke sini (jika ada)
    

    [Header("Settings")]
    public string mainMenuSceneName = "MainMenu";
                                            // Nama scene Main Menu — harus sama persis
                                            // dengan nama file scene di Unity

    private bool isPaused = false;          // Status pause saat ini

    void Awake()
    {
        if (instance == null)
        {
            instance = this;                // Daftarkan sebagai satu-satunya PauseManager
        }
        else
        {
            Destroy(gameObject);            // Hancurkan duplikat jika sudah ada
            return;
        }
    }

    void Start()
    {
        // Pastikan semua panel tersembunyi saat game mulai
        // Ini sebagai backup jika lupa nonaktifkan di Inspector
        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (howToPlayPanel != null) howToPlayPanel.SetActive(false);
        if (howToPlayPanel2 != null) howToPlayPanel2.SetActive(false);
    }

    // =============================================
    // FUNGSI PAUSE & RESUME
    // =============================================

    public void OpenPause()                 // Dipanggil oleh tombol Pause di layar
    {
        if (isPaused) return;               // Sudah pause? Abaikan

        isPaused = true;
        pausePanel.SetActive(true);         // Tampilkan pause panel

        if (pauseButton != null) pauseButton.SetActive(false);
                                            // Sembunyikan tombol pause agar tidak bisa dipencet lagi
        Time.timeScale = 0f;               // Bekukan semua physics, animation, timer
                                            // Nilai 0 = game berhenti total
                                            // Nilai 1 = kecepatan normal
    }

    public void ClosePause()               // Dipanggil oleh tombol X dan Resume
    {
        if (!isPaused) return;              // Belum pause? Abaikan

        isPaused = false;

        // Sembunyikan semua panel — termasuk settings/howtoplay jika masih terbuka
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        howToPlayPanel2.SetActive(false);

        if (pauseButton != null) pauseButton.SetActive(true);
                                            // Tampilkan kembali tombol pause

        Time.timeScale = 1f;               // Kembalikan game ke kecepatan normal
    }

    public bool IsPaused()                 // Dipanggil oleh script lain yang perlu cek status
    {
        return isPaused;
    }

    // =============================================
    // FUNGSI NAVIGASI PANEL
    // =============================================

    public void OpenSettings()             // Dipanggil oleh BtnSettings
    {
        pausePanel.SetActive(false);        // Sembunyikan pause panel
        settingsPanel.SetActive(true);      // Tampilkan settings panel
    }

    public void CloseSettings()            // Dipanggil oleh BtnBack di Settings
    {
        settingsPanel.SetActive(false);     // Sembunyikan settings
        pausePanel.SetActive(true);         // Kembali ke pause panel
    }

    public void OpenHowToPlay()            // Dipanggil oleh BtnHowToPlay
    {
        pausePanel.SetActive(false);
        howToPlayPanel.SetActive(true);
    }

    public void CloseHowToPlay()           // Dipanggil oleh BtnBack di HowToPlay
    {
        howToPlayPanel.SetActive(false);
        howToPlayPanel2.SetActive(false);
        pausePanel.SetActive(true);
    }

    // =============================================
    // FUNGSI MAIN MENU
    // =============================================

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;               // WAJIB reset sebelum pindah scene
                                            // Tanpa ini, scene Main Menu ikut freeze
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // =============================================
    // SAFETY — reset timeScale jika object dihancurkan
    // =============================================

    void OnDestroy()
    {
        Time.timeScale = 1f;               // Jaga-jaga agar tidak stuck di timeScale 0
    }
}
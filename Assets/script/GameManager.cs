using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton — supaya script lain bisa akses GameManager dari mana saja
    public static GameManager instance;

    [Header("Referensi Player")]
    public HealthSystem playerHealth;   // Drag komponen HealthSystem dari Player root ke sini

    [Header("Pengaturan Waktu")]
    public float gameDuration = 120f;   // Durasi permainan dalam detik (2 menit)

    [Header("Nama Scene Ending")]
    public string ending1Scene = "Ending1"; // HP > 80% — selamat sempurna
    public string ending2Scene = "Ending2"; // HP 1–80% — selamat tapi terluka
    public string ending3Scene = "Ending3"; // HP 0% atau timer habis — gagal

    // Variabel internal
    private float timer;        // Penghitung waktu mundur
    private bool gameOver;      // Flag agar ending tidak dipanggil dua kali

    void Awake()
    {
        // Pastikan hanya ada 1 GameManager di scene (Singleton pattern)
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        timer = gameDuration;
        gameOver = false;

        // Simpan level aktif berdasarkan nama scene yang sedang berjalan
        // Dipakai oleh EvaluasiManager untuk tahu harus Coba Lagi ke level mana
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.StartsWith("Level 2"))
            GameResult.currentLevel = 2;    // ← TAMBAHAN BARU
        else
            GameResult.currentLevel = 1;    // ← TAMBAHAN BARU

        if (playerHealth != null)
        {
            // Dengarkan event onDeath — dipanggil otomatis saat HP = 0
            playerHealth.onDeath.AddListener(OnPlayerDeath);
        }
        else
        {
            Debug.LogError("[GameManager] playerHealth belum di-assign di Inspector!");
        }

        // Pastikan waktu berjalan normal (penting kalau sebelumnya pernah di-pause)
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (gameOver) return;

        // Hitung mundur timer setiap frame
        timer -= Time.deltaTime;

        // Timer habis = gagal evakuasi → Ending 3
        if (timer <= 0f)
        {
            timer = 0f;
            TriggerEnding3();
        }
    }

    // =============================================
    // Dipanggil oleh ExitPoint / ExitTrigger saat player keluar rumah
    // =============================================
    public void OnPlayerExit()
    {
        if (gameOver) return;

        float hpPercent = playerHealth.GetHealthPercent() * 100f;

        Debug.Log("[GameManager] Player keluar! HP: " + hpPercent + "%");

        if (hpPercent > 80f)
            TriggerEnding1();
        else
            TriggerEnding2();
    }

    // Dipanggil otomatis oleh event onDeath di HealthSystem saat HP = 0
    private void OnPlayerDeath()
    {
        if (gameOver) return;

        Debug.Log("[GameManager] Player mati! Menuju Ending 3.");
        TriggerEnding3();
    }

    // =============================================
    // Semua transisi scene WAJIB lewat LoadingScreen
    // =============================================
    private void TriggerEnding1()
    {
        if (gameOver) return;
        gameOver = true;
        Time.timeScale = 1f;

        GameResult.finalHpPercent     = playerHealth.GetHealthPercent() * 100f;
        GameResult.finalTimeRemaining = gameDuration - timer;
        GameResult.endingNumber       = 1;

        LoadingScreenManager.targetScene = ending1Scene;
        SceneManager.LoadScene("LoadingScreen");
    }

    private void TriggerEnding2()
    {
        if (gameOver) return;
        gameOver = true;
        Time.timeScale = 1f;

        GameResult.finalHpPercent     = playerHealth.GetHealthPercent() * 100f;
        GameResult.finalTimeRemaining = gameDuration - timer;
        GameResult.endingNumber       = 2;

        LoadingScreenManager.targetScene = ending2Scene;
        SceneManager.LoadScene("LoadingScreen");
    }

    private void TriggerEnding3()
    {
        if (gameOver) return;
        gameOver = true;
        Time.timeScale = 1f;

        GameResult.finalHpPercent     = playerHealth.GetHealthPercent() * 100f;
        GameResult.finalTimeRemaining = gameDuration - timer;
        GameResult.endingNumber       = 3;

        LoadingScreenManager.targetScene = ending3Scene;
        SceneManager.LoadScene("LoadingScreen");
    }

    public float GetTimeRemaining()
    {
        return timer;
    }
}
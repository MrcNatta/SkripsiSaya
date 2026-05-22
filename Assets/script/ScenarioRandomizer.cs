using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenarioRandomizer : MonoBehaviour
{
    // Array berisi nama semua scene skenario yang tersedia
    // Kamu bisa tambah atau kurangi nama scene di sini lewat Inspector
    [Header("Daftar Scene Skenario")]
    public string[] scenarioScenes = { "Level1A", "Level1B", "Level1C" };

    // Dipanggil saat tombol Play ditekan
    public void OnPlayClicked()
    {
        // Pastikan array tidak kosong sebelum dipakai
        if (scenarioScenes == null || scenarioScenes.Length == 0)
        {
            Debug.LogError("[ScenarioRandomizer] Tidak ada scene yang diisi di Inspector!");
            return;
        }

        // Pilih angka acak antara 0 sampai jumlah scene - 1
        // Contoh: ada 3 scene → acak antara 0, 1, atau 2
        int randomIndex = Random.Range(0, scenarioScenes.Length);

        // Ambil nama scene berdasarkan index acak tadi
        string chosenScene = scenarioScenes[randomIndex];

        Debug.Log("[ScenarioRandomizer] Scene terpilih: " + chosenScene);

        // Set target scene di LoadingScreenManager, lalu muat LoadingScreen
        // WAJIB pakai cara ini — jangan langsung LoadScene ke Level1A/1B/1C
        LoadingScreenManager.targetScene = chosenScene;
        SceneManager.LoadScene("LoadingScreen");
    }
}
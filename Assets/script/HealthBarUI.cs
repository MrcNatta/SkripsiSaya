using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Slider healthSlider;         // Drag HealthSlider dari Hierarchy ke sini
    public HealthSystem playerHealth;   // Drag Player dari Hierarchy ke sini

    void Start()
    {
        if (playerHealth == null)
        {
            Debug.LogError("HealthBarUI: playerHealth belum di-assign!");
            return;
        }

        playerHealth.onHealthChanged.AddListener(UpdateBar);
        // Daftarkan fungsi UpdateBar sebagai pendengar event onHealthChanged
        // Setiap kali TakeDamage dipanggil di HealthSystem,
        // fungsi ini otomatis dipanggil dengan nilai persentase terbaru

        UpdateBar(1f);                  // Set tampilan penuh di awal
    }

    void UpdateBar(float healthPercent)
    {
        if (healthSlider != null)
            healthSlider.value = healthPercent;
        // healthPercent = 0.0 (mati) sampai 1.0 (penuh)
        // Slider akan otomatis menyesuaikan tampilannya
    }

    void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.onHealthChanged.RemoveListener(UpdateBar);
        // Bersihkan listener saat object dihancurkan
        // Mencegah error saat scene berganti
    }
}
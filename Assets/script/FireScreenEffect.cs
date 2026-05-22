using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Script ini dipasang ke GameObject FireDamageOverlay (UI Image)
// Tugasnya: tampilkan efek merah di layar saat player kena api

public class FireScreenEffect : MonoBehaviour
{
    [Header("Pengaturan Efek")]
    public float maxAlpha = 0.5f;           // Seberapa merah maksimal layar
                                            // 0 = transparan, 1 = merah penuh
                                            // 0.5 cukup terlihat tapi tidak terlalu mengganggu

    public float fadeInSpeed = 8f;          // Seberapa cepat merah muncul saat kena api
    public float fadeOutSpeed = 2f;         // Seberapa cepat merah hilang saat keluar api
                                            // Sengaja lebih lambat agar efek terasa "panas"

    private Image overlayImage;             // Referensi ke komponen Image di GameObject ini
    private float targetAlpha = 0f;         // Target alpha yang ingin dicapai
                                            // 0 = tidak kena api, maxAlpha = kena api
    private bool isTakingDamage = false;    // Status apakah player sedang kena api

    void Awake()
    {
        // Ambil komponen Image dari GameObject yang sama
        overlayImage = GetComponent<Image>();

        if (overlayImage == null)
            Debug.LogError("FireScreenEffect: Tidak ada komponen Image!");

        // Pastikan alpha mulai dari 0 (tidak terlihat)
        SetAlpha(0f);
    }

    void Update()
    {
        // Setiap frame, gerakkan alpha perlahan ke arah targetAlpha
        // Ini yang menciptakan efek fade in / fade out yang smooth

        float currentAlpha = overlayImage.color.a;
                                            // Ambil nilai alpha sekarang

        float speed = isTakingDamage ? fadeInSpeed : fadeOutSpeed;
                                            // Kalau kena api: pakai fadeInSpeed (cepat)
                                            // Kalau tidak: pakai fadeOutSpeed (lambat)

        float newAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, speed * Time.deltaTime);
                                            // MoveTowards: gerakkan nilai dari A ke B
                                            // dengan kecepatan tertentu per frame
                                            // Time.deltaTime = waktu antar frame
                                            // agar kecepatan sama di semua device

        SetAlpha(newAlpha);                 // Terapkan alpha baru ke Image
    }

    // Dipanggil dari FireDamage.cs saat player MASUK area api
    public void StartEffect()
    {
        isTakingDamage = true;              // Aktifkan status kena api
        targetAlpha = maxAlpha;             // Target: merah maksimal
    }

    // Dipanggil dari FireDamage.cs saat player KELUAR area api
    public void StopEffect()
    {
        isTakingDamage = false;             // Nonaktifkan status kena api
        targetAlpha = 0f;                   // Target: hilang sepenuhnya
    }

    // Helper function untuk set alpha Image
    private void SetAlpha(float alpha)
    {
        Color color = overlayImage.color;   // Ambil warna sekarang
        color.a = alpha;                    // Ubah nilai alpha saja, warna RGB tidak berubah
        overlayImage.color = color;         // Terapkan kembali ke Image
    }
}
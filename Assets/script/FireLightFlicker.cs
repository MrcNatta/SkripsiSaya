using UnityEngine;

public class FireLightFlicker : MonoBehaviour
{
    // Referensi ke komponen Light yang ada di object ini
    private Light fireLight;

    [Header("Pengaturan Kedipan")]
    // Intensitas minimum cahaya saat berkedip
    public float minIntensity = 1.5f;
    // Intensitas maksimum cahaya saat berkedip
    public float maxIntensity = 4f;
    // Seberapa cepat cahaya berkedip (makin besar makin cepat)
    public float flickerSpeed = 8f;

    void Awake()
    {
        // Ambil komponen Light dari object yang sama
        fireLight = GetComponent<Light>();

        // Kalau Light tidak ditemukan, tampilkan error
        if (fireLight == null)
            Debug.LogError("FireLightFlicker: Tidak ada komponen Light di " + gameObject.name);
    }

    void Update()
    {
        if (fireLight == null) return;

        // Mathf.PerlinNoise menghasilkan angka acak yang halus (0.0 - 1.0)
        // Time.time * flickerSpeed = membuat nilai berubah seiring waktu
        // Setiap FirePoint diberi offset berbeda (GetInstanceID) supaya tidak berkedip bersamaan
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, GetInstanceID());

        // Lerp = interpolasi antara minIntensity dan maxIntensity berdasarkan noise
        fireLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}
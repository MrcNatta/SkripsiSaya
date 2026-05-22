using UnityEngine;
using UnityEngine.UI;               // Untuk Slider

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;         // Drag VolumeSlider ke sini di Inspector

    void Start()
    {
        if (volumeSlider == null)
        {
            Debug.LogError("VolumeControl: volumeSlider belum di-assign!");
            return;
        }

        // Ambil nilai volume yang tersimpan dari session sebelumnya
        // PlayerPrefs = sistem penyimpanan sederhana bawaan Unity
        // Jika belum pernah disimpan, gunakan nilai default 1 (penuh)
        float saved = PlayerPrefs.GetFloat("MasterVolume", 1f);
        volumeSlider.value = saved;
        ApplyVolume(saved);             // Langsung terapkan saat panel dibuka

        volumeSlider.onValueChanged.AddListener(ApplyVolume);
                                        // Setiap kali slider digeser, panggil ApplyVolume
    }

    void ApplyVolume(float value)
    {
        AudioListener.volume = value;   // AudioListener.volume mengatur semua suara di scene
                                        // 0 = bisu, 1 = penuh
                                        // Ini cara paling sederhana tanpa perlu AudioMixer

        PlayerPrefs.SetFloat("MasterVolume", value);
                                        // Simpan nilai — akan diingat saat game dibuka kembali
        PlayerPrefs.Save();             // Paksa simpan sekarang, bukan tunggu game ditutup
    }

    void OnDestroy()
    {
        if (volumeSlider != null)
            volumeSlider.onValueChanged.RemoveListener(ApplyVolume);
                                        // Bersihkan listener saat object dihancurkan
    }
}